namespace TestShared.Tests;

using TestShared;
using TestShared.Fixtures;

//NOTE: These tests ensure that Testing Setup and Teardown calls are called the correct and expected number of times between runs. 
//And verify that the SharedContextTestFixture keeps the same testingcontext between tests.
//These are strange tests, and are not good examples of how to write tests, but, we're testing the testing framework, 
//And who tests the tests?  This test.
[Category(TestingCategoryConstants.nUnitFrameworkTests)]
public class SharedContextTestFixturesTest : SharedContextTestFixture
{
    //If all these tests pass, that means that using the base test fixture on future tests ensures that the fixture creates a context 
    //at the start, and always shares the context unless intentionally reset. Using the EnvironmentName here proves that if we create
    //a context with an environment name, that name should persist through tests, since that is only established in the setup. 
    private bool _firstTestHasBeenRan;
    private const string _environmentNameUsedInFirstUnitTest = $"The Environment Name for the test context was applied in :{nameof(SharedContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName)}";

    [OneTimeSetUp]
    public async Task RunBeforeTheseTests()
    {
        await TestingContext.TearDownTestContext();
    }

    [Order(1)]
    [Test]
    public async Task SharedContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName()
    {
        //Arrange, Act & Assert

        this._firstTestHasBeenRan = true;
        await TestingContext.SetupTestContext(_environmentNameUsedInFirstUnitTest);
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(_environmentNameUsedInFirstUnitTest));
    }

    [Order(2)]
    [Test]
    public async Task SharedContextTestFixtureTestSetUpTestSetUp_Test2DoNothing_EnvironmentNameIsStillTheSameAsPriorTest()
    {
        if (this._firstTestHasBeenRan)
        {
            //Arrange, Act & Assert
            Assert.That(TestingContext.EnvironmentName, Is.EqualTo(_environmentNameUsedInFirstUnitTest));
        }
        else
        {
            Assert.Inconclusive($"This test must be ran immediately after {nameof(SharedContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName)}.");
        }
    }
}





[Category(TestingCategoryConstants.nUnitFrameworkTests)]
public class SharedContextTestFixtureSetupAndTearDownTests : SharedContextTestFixture
{
    //NOTE: These tests ensure that Testing Setup and Teardown calls are called the correct and expected number of times between runs. 
    //These are strange tests, and are not good examples of how to write tests, but, we're testing the testing framework. 

    private int _timesContextSetupHasBeenCalled;
    private int _timesContextTeardownHasBeenCalled;
    private int _timesContextResetHasBeenCalled;
    private bool _firstTestHasBeenRan;
    private bool _secondTestHasBeenRan;
    private bool _thirdTestHasBeenRan;

    [Order(1)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test1_ContextShouldHaveBeenSetupAtLeastOnceByTheTimeThisTestIsRan()
    {
        //Arrange, Act & Assert
        this._timesContextSetupHasBeenCalled = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        this._timesContextTeardownHasBeenCalled = TestingContext.__metadata_NumberOfTearDownTestContextCalls;
        this._timesContextResetHasBeenCalled = TestingContext.__metadata_NumberOfResetTestContextCalls;

        Assert.That(TestingContext.__metadata_NumberOfSetupTestContextCalls, Is.GreaterThanOrEqualTo(1));
        this._firstTestHasBeenRan = true;
    }

    [Order(2)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test2_SetupAndTearDownShouldNotHaveBeenCalled()
    {

        if (this._firstTestHasBeenRan)
        {
            //Arrange, Act & Assert            
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - this._timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - this._timesContextTeardownHasBeenCalled;
            var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - this._timesContextResetHasBeenCalled;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.Zero);
                this._secondTestHasBeenRan = true;
            }
        }
        else
        {
            Assert.Inconclusive($"This test must be ran immediately after {nameof(SharedContextTestFixtureSetupAndTearDownTests_Test1_ContextShouldHaveBeenSetupAtLeastOnceByTheTimeThisTestIsRan)}.");
        }
    }

    [Order(3)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test3_SetupAndTearDownStillShouldNotHaveBeenCalled()
    {

        if (this._secondTestHasBeenRan)
        {
            //Arrange, Act & Assert
            //NOTE: This test makes sure that nothing has changed between the first, and third test.             
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - this._timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - this._timesContextTeardownHasBeenCalled;
            var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - this._timesContextResetHasBeenCalled;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.Zero);
                this._thirdTestHasBeenRan = true;
            }
        }
        else
        {
            Assert.Inconclusive($"This test must be ran immediately after {nameof(SharedContextTestFixtureSetupAndTearDownTests_Test2_SetupAndTearDownShouldNotHaveBeenCalled)}.");
        }
    }
    [Order(4)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test4CallsReset_ResetAndSetupAndTeardownShouldHaveBeenCalledOnce()
    {

        if (this._thirdTestHasBeenRan)
        {
            //Arrange, Act & Assert
            //NOTE: ResetTest should call teardown and setup, along with the base test calling setup
            await TestingContext.ResetTestContext();
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - this._timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - this._timesContextTeardownHasBeenCalled;
            var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - this._timesContextResetHasBeenCalled;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.EqualTo(1));
                Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.EqualTo(1));
                Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.EqualTo(1));
            }
        }
        else
        {
            Assert.Inconclusive($"This test must be ran immediately after {nameof(SharedContextTestFixtureSetupAndTearDownTests_Test3_SetupAndTearDownStillShouldNotHaveBeenCalled)}.");
        }
    }
}