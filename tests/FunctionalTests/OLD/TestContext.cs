//namespace FunctionalTests;

//public partial class TestContext
//{
//    private static CustomWebApplicationFactory _webApplicationfactory = null!;
//    /// <summary>
//    /// This method will setup the testing context.  This normally is ran at the beginning of test runs. But can be manually called if wanted. 
//    /// </summary>
//    /// <returns></returns>
//    //public static async Task SetupTestContext(string? environmentName = null)
//    public static async Task SetupTestContext()
//    {
//        _webApplicationfactory = new CustomWebApplicationFactory();

//        //_scopeFactory = _webApplicationfactory.Services.GetRequiredService<IServiceScopeFactory>();

//        ////Delete the testing database, then create it so it's always new and fresh. 
//        //var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        //var databaseConnection = context.Database.GetDbConnection();

//        //if (databaseConnection.Database != "Kytc.Boilerplate.Template.UnitTest")
//        //{
//        //    //This is only a santity check, since we're about to delete the configured database, we want to make doubly sure that we're only
//        //    //deleting the testing database.
//        //    throw new InvalidOperationException($"While setting up the testing context, the database {databaseConnection.Database} was set to be deleted instead of the expected Kytc.Boilerplate.Template.UnitTest database.");
//        //}

//        //await context.Database.EnsureDeletedAsync();
//        //await context.Database.EnsureCreatedAsync();
//    }

//    /// <summary>
//    /// This method will tear down the testing context.  This normally is ran only after all tests are complete. But can be manually called if wanted. 
//    /// </summary>
//    /// <returns></returns>
//    //[OneTimeTearDown]
//    public static async Task TearDownTestContext()
//    {
//        //_scopeFactory = null;
//    }
//}
