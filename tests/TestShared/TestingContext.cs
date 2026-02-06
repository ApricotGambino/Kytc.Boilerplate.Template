namespace TestShared;

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
    public static bool __metadata_IsContextSetup { get; private set; }
    public static string? EnvironmentName { get; private set; }

    public static TestCustomWebApplicationFactory? WebApplicationFactory { get; private set; }
    public static IServiceScopeFactory? ScopeFactory { get; private set; }
    public static IServiceProvider? ServiceProvider { get; private set; }



    /// <summary>
    /// This method will setup the testing context.  This normally is ran at the beginning of test runs. But can be manually called if wanted. 
    /// </summary>
    /// <param name="environmentName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task SetupTestContextAsync(string? environmentName = null)
    {
        //TODO: Make sure everything is tore down
        if (__metadata_IsContextSetup)
        {
            //NOTE: We want to make sure that if setup is being called, it's in the context of it being 'fresh', this
            //helps prevent tests from setting up context without intentionality. 
            throw new InvalidOperationException($"Testing Context setup was attempted without ensuring teardown, make sure to teardown context prior to setting up, this can be easily done by using the {nameof(ResetTestContextAsync)} method.");
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                EnvironmentName = environmentName;
            }
            else
            {
                EnvironmentName = TestingConstants.EnvironmentName;
            }

            WebApplicationFactory = new TestCustomWebApplicationFactory(EnvironmentName);

            ScopeFactory = WebApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();

            //Delete the testing database, then create it so it's always new and fresh. 
            //var a = ScopeFactory.CreateScope().ServiceProvider;
            //var test = ScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILoggingService>();
            ServiceProvider = ScopeFactory.CreateScope().ServiceProvider;
            var context = ServiceProvider.GetRequiredService<TestingDatabaseContext>();
            var databaseConnection = context.Database.GetDbConnection();

            //NOTE: Not using the TestingConstants.UnitTestDatabaseName here because I want to make sure that you're really sure if you're going to modify the expected database name for unit tests.
            if (databaseConnection.Database != "Kytc.Boilerplate.Template.UnitTest"
                && databaseConnection.Database != "Kytc.Boilerplate.Template.PerformanceUnitTest")
            {
                //This is only a santity check, since we're about to delete the configured database, we want to make doubly sure that we're only
                //deleting the testing database.
                throw new InvalidOperationException($"While setting up the testing context, the database {databaseConnection.Database} was set to be deleted instead of an exepected UnitTest database.");
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
            __metadata_IsContextSetup = true;
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
            EnvironmentName = TestingConstants.EnvironmentName;
        }
        finally
        {
            __metadata_IsContextSetup = false;
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

    //public static GetService<T>()
    //{
    //    return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    //}

    public static T GetService<T>() where T : notnull
    {
        if (ServiceProvider == null)
        {
            throw new InvalidOperationException($"The ServiceProvider is null, make sure you've initialized the TestingContext using {nameof(SetupTestContextAsync)}");
        }
        return ServiceProvider.GetRequiredService<T>();
    }

    //private static TestCustomWebApplicationFactory _webApplicationfactory = null!;
    //private static IServiceScopeFactory? _scopeFactory = null!;

    ///// <summary>
    ///// This method will setup the testing context.  This normally is ran at the beginning of test runs. But can be manually called if wanted. 
    ///// </summary>
    ///// <returns></returns>
    //public static async Task SetupTestContext(string? environmentName = null)
    //{
    //    _webApplicationfactory = new TestCustomWebApplicationFactory(environmentName);

    //    _scopeFactory = _webApplicationfactory.Services.GetRequiredService<IServiceScopeFactory>();

    //    //Delete the testing database, then create it so it's always new and fresh. 
    //    var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //    var databaseConnection = context.Database.GetDbConnection();

    //    if (databaseConnection.Database != "Kytc.Boilerplate.Template.UnitTest")
    //    {
    //        //This is only a santity check, since we're about to delete the configured database, we want to make doubly sure that we're only
    //        //deleting the testing database.
    //        throw new InvalidOperationException($"While setting up the testing context, the database {databaseConnection.Database} was set to be deleted instead of the expected Kytc.Boilerplate.Template.UnitTest database.");
    //    }

    //    await context.Database.EnsureDeletedAsync();
    //    await context.Database.EnsureCreatedAsync();
    //}

    ///// <summary>
    ///// This method returns the private scopeFactory.
    ///// </summary>
    ///// <returns></returns>
    //public static IServiceScopeFactory? GetScopeFactory() =>
    //    //We want this class to be static, but because of that we can't initialize scopeFactory in a constructor.
    //    //Because scopeFactory relies on the TestCustomWebApplicationFactory, we have to initialize it in the SetupTestContext method.
    //    //This prevents us from making the scopeFactory a constant.
    //    //Because this can't be a constant, if we make this value public, we're going to get compiler warnings telling us
    //    //that this value shouldn't be public, because public static methods are not threadsafe.
    //    //We really only ever want to set this once in the SetupTestContext method, but in order to make the SetupTestContext method testable,
    //    //We need to expose it somehow.  This was a lot of text to explain this, and hopefully someone has a better idea than this. 
    //    _scopeFactory;

    ///// <summary>
    ///// This method returns the private scopeFactory.
    ///// </summary>
    ///// <returns></returns>
    //public static TestCustomWebApplicationFactory? GetTestCustomWebApplicationFactory() =>
    //    _webApplicationfactory;

    ///// <summary>
    ///// Get's the database context.  We do this so the DB context is grabbed fresh every time, since storing it as a static would lead to issues. 
    ///// </summary>
    ///// <returns></returns>
    //public static ApplicationDbContext GetDbContext()
    //{
    //    if (_scopeFactory == null)
    //    {
    //        throw new InvalidOperationException("The TestContext has not been setup. Call SetupTestContext before trying to access the database context.");
    //    }

    //    var scope = _scopeFactory.CreateScope();
    //    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //    return context;
    //}


    ///// <summary>
    ///// This method will tear down the testing context.  This normally is ran only after all tests are complete. But can be manually called if wanted. 
    ///// </summary>
    ///// <returns></returns>
    ////[OneTimeTearDown]
    //public static async Task TearDownTestContext()
    //{
    //    _scopeFactory = null;
    //}

    ///// <summary>
    ///// This method allows us to easily fetch data from the database defined in the testing context. 
    ///// </summary>
    ///// <typeparam name="TEntity"></typeparam>
    ///// <param name="keyValues"></param>
    ///// <returns></returns>
    //public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
    //    where TEntity : class
    //{
    //    var context = GetDbContext();

    //    return await context.FindAsync<TEntity>(keyValues);
    //}

    ///// <summary>
    ///// This method allows us to easily add data to the database defined in the testing context. 
    ///// </summary>
    ///// <typeparam name="TEntity"></typeparam>
    ///// <param name="entity"></param>
    ///// <returns></returns>
    //public static async Task<TEntity> AddAsync<TEntity>(TEntity entity)
    //    where TEntity : class
    //{
    //    var context = GetDbContext();

    //    context.Add(entity);

    //    await context.SaveChangesAsync();

    //    return entity;
    //}

    ///// <summary>
    ///// This method allows us to count entities from the database defined in the testing context. 
    ///// </summary>
    ///// <typeparam name="TEntity"></typeparam>
    ///// <returns></returns>
    //public static async Task<int> CountAsync<TEntity>() where TEntity : class
    //{
    //    var context = GetDbContext();

    //    return await context.Set<TEntity>().CountAsync();
    //}
}
