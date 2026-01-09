namespace Kytc.Boilerplate.Template.IntegrationTests;


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// This class allows us to access the context of all testing needs.  
/// </summary>
public static class TestContext
{
    private static TestCustomWebApplicationFactory _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;

    /// <summary>
    /// This method will setup the testing context.  This normally is ran at the beginning of test runs. But can be manually called if wanted. 
    /// </summary>
    /// <returns></returns>
    public static async Task SetupTestContext()
    {
        _factory = new TestCustomWebApplicationFactory();

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        //Delete the testing database, then create it so it's always new and fresh. 
        var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }


    /// <summary>
    /// This method will tear down the testing context.  This normally is ran only after all tests are complete. But can be manually called if wanted. 
    /// </summary>
    /// <returns></returns>
    //[OneTimeTearDown]
    public static async Task TearDownTestContext()
    {
        //This is intentionally left blank, feel free to add whatever may assist you with testing. 
    }

    /// <summary>
    /// This method allows us to easily fetch data from the database defined in the testing context. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="keyValues"></param>
    /// <returns></returns>
    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    /// <summary>
    /// This method allows us to easily add data to the database defined in the testing context. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// This method allows us to count entities from the database defined in the testing context. 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }
}
