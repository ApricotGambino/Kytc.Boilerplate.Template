namespace TestingInfrastructureTests;

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

    [OneTimeSetUp]
    public Task RunBeforeTheseTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }

    [Order(1)]
    [Test]
    public async Task SharedContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName()
    {
        //Arrange, Act & Assert

        _firstTestHasBeenRan = true;
        await TestingContext.SetupTestContextAsync(TestingConstants.TestingEnvironmentName);
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }

    [Order(2)]
    [Test]
    public async Task SharedContextTestFixtureTestSetUpTestSetUp_Test2DoNothing_EnvironmentNameIsStillTheSameAsPriorTest()
    {
        if (_firstTestHasBeenRan)
        {
            //Arrange, Act & Assert
            Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
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
        _timesContextSetupHasBeenCalled = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        _timesContextTeardownHasBeenCalled = TestingContext.__metadata_NumberOfTearDownTestContextCalls;
        _timesContextResetHasBeenCalled = TestingContext.__metadata_NumberOfResetTestContextCalls;

        Assert.That(TestingContext.__metadata_NumberOfSetupTestContextCalls, Is.GreaterThanOrEqualTo(1));
        _firstTestHasBeenRan = true;
    }

    [Order(2)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test2_SetupAndTearDownShouldNotHaveBeenCalled()
    {

        if (_firstTestHasBeenRan)
        {
            //Arrange, Act & Assert            
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _timesContextTeardownHasBeenCalled;
            var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - _timesContextResetHasBeenCalled;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.Zero);
                _secondTestHasBeenRan = true;
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

        if (_secondTestHasBeenRan)
        {
            //Arrange, Act & Assert
            //NOTE: This test makes sure that nothing has changed between the first, and third test.             
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _timesContextTeardownHasBeenCalled;
            var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - _timesContextResetHasBeenCalled;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.Zero);
                Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.Zero);
                _thirdTestHasBeenRan = true;
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

        if (_thirdTestHasBeenRan)
        {
            //Arrange, Act & Assert
            //NOTE: ResetTest should call teardown and setup, along with the base test calling setup
            await TestingContext.ResetTestContextAsync(TestingContext.EnvironmentName);
            var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _timesContextSetupHasBeenCalled;
            var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _timesContextTeardownHasBeenCalled;
            var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - _timesContextResetHasBeenCalled;
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
