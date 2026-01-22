//namespace IntegrationTests.UniqueContextTests;

//using TestShared;

///// <summary>
///// This nUnit test fixture is used for tests that create a new context for each test.
///// That means these tests are going to be slow, because they are going to create and delete
///// the database for each test.  Sorry about that, but that's what we're testing. 
///// </summary>
//public class UniqueContextTestTestFixture : BaseTestFixture
//{
//    [SetUp]
//    public override async Task TestSetUp()
//    {
//        //We want to make sure that the context is fresh prior to each test here. 
//        await TestingContext.TearDownTestContext();
//    }
//}
