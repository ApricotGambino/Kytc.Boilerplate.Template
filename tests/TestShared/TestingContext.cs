namespace TestShared;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// This class allows us to access the context of all testing needs.  
/// </summary> 

public static class TestingContext
{
    /// <summary>
    /// Metadata values are used for testing analysis, these should not be reset 
    /// </summary>
    public static int __metadata_NumberOfSetupTestContextCalls { get; private set; }
    public static int __metadata_NumberOfTearDownTestContextCalls { get; private set; }
    public static int __metadata_NumberOfResetTestContextCalls { get; private set; }
    public static bool __metadata_ContextHasBeenSetup { get; private set; }


    public static string? EnvironmentName { get; private set; }
    public static TestCustomWebApplicationFactory? WebApplicationFactory { get; private set; }
    public static IServiceScopeFactory? ScopeFactory { get; private set; }
    public static IServiceProvider? ServiceProvider { get; private set; }

    /// <summary>
    /// This method will setup the testing context
    /// </summary>
    /// <param name="environmentName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task SetupTestContextAsync(string? environmentName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(environmentName);

        if (__metadata_ContextHasBeenSetup)
        {
            //NOTE: We want to make sure that if setup is being called, it's in the context of it being 'fresh', this
            //helps prevent tests from setting up context without intentionality. 
            throw new InvalidOperationException($"Testing Context setup was attempted without ensuring teardown, make sure to teardown context prior to setting up, this can be easily done by using the {nameof(ResetTestContextAsync)} method.");
        }

        try
        {
            EnvironmentName = environmentName;

            WebApplicationFactory = new TestCustomWebApplicationFactory(EnvironmentName);

            ScopeFactory = WebApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();

            ServiceProvider = ScopeFactory.CreateScope().ServiceProvider;

            //Delete the testing database, then create it so it's always new and fresh. 
            var context = GetService<TestingDatabaseContext>();
            var databaseConnection = context.Database.GetDbConnection();

            //NOTE: Not using the TestingConstants names here because I want to make sure that you're really sure if you're going to modify the expected database name for unit tests.
            if (databaseConnection.Database != "Kytc.Boilerplate.Template.Testing"
                && databaseConnection.Database != "Kytc.Boilerplate.Template.BenchmarkTests")
            {
                //This is only a santity check, since we're about to delete the configured database, we want to make doubly sure that we're only
                //deleting the testing database.
                throw new InvalidOperationException($"While setting up the testing context, the database {databaseConnection.Database} was set to be deleted instead of an exepected Testing database. You're welcome for the catch.");
            }

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception)
        {
            await TearDownTestContextAsync();
            throw;
        }
        finally
        {
            __metadata_NumberOfSetupTestContextCalls++;
            __metadata_ContextHasBeenSetup = true;
        }
    }

    /// <summary>
    /// This method will tear down the testing context.  This normally is ran only after all tests are complete. But can be manually called if wanted. 
    /// </summary>
    /// <returns></returns>
    public static async Task TearDownTestContextAsync()
    {
        try
        {
            EnvironmentName = string.Empty;
            WebApplicationFactory = null;
            ScopeFactory = null;
            ServiceProvider = null;
        }
        finally
        {
            __metadata_ContextHasBeenSetup = false;
            __metadata_NumberOfTearDownTestContextCalls++;
        }
    }

    /// <summary>
    /// This will reset the context by first tearing it down, then creating it again. 
    /// </summary>
    /// <param name="environmentName"></param>
    /// <returns></returns>
    public static async Task ResetTestContextAsync(string? environmentName = null)
    {
        try
        {
            await TearDownTestContextAsync();
            await SetupTestContextAsync(environmentName);
        }
        finally
        {
            __metadata_NumberOfResetTestContextCalls++;
        }
    }

    /// <summary>
    /// Gets the configured service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T GetService<T>() where T : notnull
    {
        if (ServiceProvider == null)
        {
            throw new InvalidOperationException($"The ServiceProvider is null, make sure you've initialized the TestingContext using {nameof(SetupTestContextAsync)}");
        }

        var foundService = ServiceProvider.GetService<T>();

        if (foundService is null)
        {
            throw new InvalidOperationException($"Could not find the service named {nameof(T)} in the service provider, are you sure this service has been registered?");
        }

        return foundService;
    }
}
