namespace TestShared.Tests;

using TestShared.Fixtures;

//NOTE: These tests ensure that Testing Setup and Teardown calls are called the correct and expected number of times between runs. 
//And verify that the UniqueContextTestTestFixture actually creates a new context each time.
//These are strange tests, and are not good examples of how to write tests, but, we're testing the testing framework, 
//And who tests the tests?  This test.
[Category(TestingCategoryConstants.nUnitFrameworkTests)]
public class UniqueContextTestFixturesContextTests : UniqueContextTestFixture
{
    //If all these tests pass, that means that using the uniquecontext test fixture on future tests ensures that the fixture doesn't 
    //try to create a context at any point, and always tears the context down. Using the EnvironmentName here proves that if we create
    //a context with an environment name, that name should not persist through tests, since that is only established in the setup, which needs to be manually called. 
    private bool _firstTestHasBeenRan;
    private const string _environmentNameUsedInFirstUnitTest = $"The Environment Name for the test context was applied in :{nameof(UniqueContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName)}";

    [Order(1)]
    [Test]
    public async Task UniqueContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName()
    {
        //Arrange, Act & Assert
        this._firstTestHasBeenRan = true;
        await TestingContext.SetupTestContext(TestingConstants.IntentionallyBadEnvironmentName);
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.IntentionallyBadEnvironmentName));
    }

    [Order(2)]
    [Test]
    public async Task UniqueContextTestFixtureTestSetUp_Test2DoNothing_EnvironmentNameIsNotTheSameAsPriorTest()
    {
        if (this._firstTestHasBeenRan)
        {
            //Arrange, Act & Assert
            Assert.That(TestingContext.EnvironmentName, Is.Not.EqualTo(_environmentNameUsedInFirstUnitTest));
        }
        else
        {
            Assert.Inconclusive($"This test must be ran immediately after {nameof(UniqueContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName)}.");
        }
    }
}


[Category(TestingCategoryConstants.nUnitFrameworkTests)]
public class UniqueContextTestFixtureSetupAndTearDownTests : UniqueContextTestFixture
{
    //NOTE: These tests ensure that Testing Setup and Teardown calls are called the correct and expected number of times between runs. 
    //These are strange tests, and are not good examples of how to write tests, but, we're testing the testing framework. 

    //If all these tests pass, that means that using the uniquecontext test fixture on future tests ensures that the fixture doesn't 
    //try to create a context at any point, and tears down the context every time without you forgetting to do so manually.  
    private int _timesContextSetupHasBeenCalled;
    private int _timesContextTeardownHasBeenCalled;
    private bool _firstTestHasBeenRan;
    private bool _secondTestHasBeenRan;

    [Order(1)]
    [Test]
    public async Task UniqueContextTestFixtureSetupAndTearDownTests_Test1_DoNothing()
    {
        //Arrange, Act & Assert
        this._timesContextSetupHasBeenCalled = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        this._timesContextTeardownHasBeenCalled = TestingContext.__metadata_NumberOfTearDownTestContextCalls;

        this._firstTestHasBeenRan = true;

        Assert.Pass();
    }

    [Order(2)]
    [Test]
    public async Task UniqueContextTestFixtureSetupAndTearDownTests_Test2_SetupCalledZeroTimesAndTearDownCalledOnce()
    {

        if (this._firstTestHasBeenRan)
        {
            //Arrange, Act & Assert
            this._secondTestHasBeenRan = true;
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - this._timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - this._timesContextTeardownHasBeenCalled;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.EqualTo(1));
            }
        }
        else
        {
            Assert.Inconclusive($"This test must be ran immediately after {nameof(UniqueContextTestFixtureSetupAndTearDownTests_Test1_DoNothing)}.");
        }
    }

    [Order(3)]
    [Test]
    public async Task UniqueContextTestFixtureSetupAndTearDownTests_Test3_SetupCalledZeroTimesAndTearDownCalledTwice()
    {

        if (this._secondTestHasBeenRan)
        {
            //Arrange, Act & Assert
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - this._timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - this._timesContextTeardownHasBeenCalled;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.EqualTo(2));
            }
        }
        else
        {
            Assert.Inconclusive($"This test must be ran immediately after {nameof(UniqueContextTestFixtureSetupAndTearDownTests_Test2_SetupCalledZeroTimesAndTearDownCalledOnce)}.");
        }
    }
}
