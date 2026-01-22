//namespace IntegrationTests.IntegrationTestConfigurations;

//using System;
//using Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

///// <summary>
///// This class allows us to access the context of all testing needs.  
///// </summary>
//public static class TestContext
//{
//    private static TestCustomWebApplicationFactory _webApplicationfactory = null!;
//    private static IServiceScopeFactory? _scopeFactory = null!;

//    /// <summary>
//    /// This method will setup the testing context.  This normally is ran at the beginning of test runs. But can be manually called if wanted. 
//    /// </summary>
//    /// <returns></returns>
//    public static async Task SetupTestContext(string? environmentName = null)
//    {
//        _webApplicationfactory = new TestCustomWebApplicationFactory(environmentName);

//        _scopeFactory = _webApplicationfactory.Services.GetRequiredService<IServiceScopeFactory>();

//        //Delete the testing database, then create it so it's always new and fresh. 
//        var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        var databaseConnection = context.Database.GetDbConnection();

//        if (databaseConnection.Database != "Kytc.Boilerplate.Template.UnitTest")
//        {
//            //This is only a santity check, since we're about to delete the configured database, we want to make doubly sure that we're only
//            //deleting the testing database.
//            throw new InvalidOperationException($"While setting up the testing context, the database {databaseConnection.Database} was set to be deleted instead of the expected Kytc.Boilerplate.Template.UnitTest database.");
//        }

//        await context.Database.EnsureDeletedAsync();
//        await context.Database.EnsureCreatedAsync();
//    }

//    /// <summary>
//    /// This method returns the private scopeFactory.
//    /// </summary>
//    /// <returns></returns>
//    public static IServiceScopeFactory? GetScopeFactory() =>
//        //We want this class to be static, but because of that we can't initialize scopeFactory in a constructor.
//        //Because scopeFactory relies on the TestCustomWebApplicationFactory, we have to initialize it in the SetupTestContext method.
//        //This prevents us from making the scopeFactory a constant.
//        //Because this can't be a constant, if we make this value public, we're going to get compiler warnings telling us
//        //that this value shouldn't be public, because public static methods are not threadsafe.
//        //We really only ever want to set this once in the SetupTestContext method, but in order to make the SetupTestContext method testable,
//        //We need to expose it somehow.  This was a lot of text to explain this, and hopefully someone has a better idea than this. 
//        _scopeFactory;

//    /// <summary>
//    /// This method returns the private scopeFactory.
//    /// </summary>
//    /// <returns></returns>
//    public static TestCustomWebApplicationFactory? GetTestCustomWebApplicationFactory() =>
//        _webApplicationfactory;

//    /// <summary>
//    /// Get's the database context.  We do this so the DB context is grabbed fresh every time, since storing it as a static would lead to issues. 
//    /// </summary>
//    /// <returns></returns>
//    public static ApplicationDbContext GetDbContext()
//    {
//        if (_scopeFactory == null)
//        {
//            throw new InvalidOperationException("The TestContext has not been setup. Call SetupTestContext before trying to access the database context.");
//        }

//        var scope = _scopeFactory.CreateScope();
//        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        return context;
//    }


//    /// <summary>
//    /// This method will tear down the testing context.  This normally is ran only after all tests are complete. But can be manually called if wanted. 
//    /// </summary>
//    /// <returns></returns>
//    //[OneTimeTearDown]
//    public static async Task TearDownTestContext()
//    {
//        _scopeFactory = null;
//    }

//    /// <summary>
//    /// This method allows us to easily fetch data from the database defined in the testing context. 
//    /// </summary>
//    /// <typeparam name="TEntity"></typeparam>
//    /// <param name="keyValues"></param>
//    /// <returns></returns>
//    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
//        where TEntity : class
//    {
//        var context = GetDbContext();

//        return await context.FindAsync<TEntity>(keyValues);
//    }

//    /// <summary>
//    /// This method allows us to easily add data to the database defined in the testing context. 
//    /// </summary>
//    /// <typeparam name="TEntity"></typeparam>
//    /// <param name="entity"></param>
//    /// <returns></returns>
//    public static async Task<TEntity> AddAsync<TEntity>(TEntity entity)
//        where TEntity : class
//    {
//        var context = GetDbContext();

//        context.Add(entity);

//        await context.SaveChangesAsync();

//        return entity;
//    }

//    /// <summary>
//    /// This method allows us to count entities from the database defined in the testing context. 
//    /// </summary>
//    /// <typeparam name="TEntity"></typeparam>
//    /// <returns></returns>
//    public static async Task<int> CountAsync<TEntity>() where TEntity : class
//    {
//        var context = GetDbContext();

//        return await context.Set<TEntity>().CountAsync();
//    }
//}
