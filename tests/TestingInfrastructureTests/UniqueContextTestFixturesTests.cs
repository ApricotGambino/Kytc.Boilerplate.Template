
using TestShared;
using TestShared.Fixtures;

namespace TestingInfrastructureTests;
/// <summary>
/// These tests ensure that <see cref="UniqueContextTestFixture"/> creates a TestingContext at the start of each test,
/// and always restarts the context for each test ran.
/// If all these tests pass, that means that using the uniquecontext test fixture on future tests ensures that the fixture doesn't
/// try to create a context at any point, and always tears the context down.
/// since that is only established in the setup, which needs to be manually called.
/// </summary>
[Category(TestingCategoryConstants.NUnitFrameworkTests)]
public class UniqueContextTestFixturesContextTests : UniqueContextTestFixture
{
    private bool _firstTestHasBeenRan;
    private readonly string _environmentNameUsedInFirstTest = TestingConstants.BenchmarkTestsEnvironmentName;

    [Order(1)]
    [Test]
    public async Task UniqueContextTestFixtureTestSetUp_Test1SetupContext_HasSpecificEnvironmentName()
    {
        //Arrange, Act & Assert
        await TestingContext.ResetTestContextAsync(_environmentNameUsedInFirstTest);
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(_environmentNameUsedInFirstTest));
        _firstTestHasBeenRan = true;
    }

    [Order(2)]
    [Test]
    public async Task UniqueContextTestFixtureTestSetUp_Test2DoNothing_EnvironmentNameIsNotTheSameAsPriorTest()
    {
        Assume.That(_firstTestHasBeenRan, Is.True);
        //Arrange, Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.Not.EqualTo(_environmentNameUsedInFirstTest));

        //The Unique Context should reset on each test run and that defaults to the 'Testing' environment.
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));

    }
}

/// <summary>
/// If all these tests pass, that means that using the uniquecontext test fixture on future tests ensures that the fixture doesn't
/// try to create a context at any point, and Setup and Teardown calls are called the correct and expected number of times between runs.
/// </summary>
[Category(TestingCategoryConstants.NUnitFrameworkTests)]
public class UniqueContextTestFixtureSetupAndTearDownTests : UniqueContextTestFixture
{
    private int _timesContextSetupHasBeenCalled;
    private int _timesContextTeardownHasBeenCalled;
    private bool _firstTestHasBeenRan;
    private bool _secondTestHasBeenRan;

    [Order(1)]
    [Test]
    public async Task UniqueContextTestFixtureSetupAndTearDownTests_Test1_DoNothing()
    {
        //Arrange, Act & Assert
        _timesContextSetupHasBeenCalled = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        _timesContextTeardownHasBeenCalled = TestingContext.__metadata_NumberOfTearDownTestContextCalls;

        _firstTestHasBeenRan = true;

        Assert.Pass();
    }

    [Order(2)]
    [Test]
    public async Task UniqueContextTestFixtureSetupAndTearDownTests_Test2_SetupCalledOnce_TearDownCalledOnce()
    {
        Assume.That(_firstTestHasBeenRan, Is.True);
        //Arrange, Act & Assert
        _secondTestHasBeenRan = true;
        var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _timesContextSetupHasBeenCalled;
        var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _timesContextTeardownHasBeenCalled;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.EqualTo(1));
            Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.EqualTo(1));
        }
    }

    [Order(3)]
    [Test]
    public async Task UniqueContextTestFixtureSetupAndTearDownTests_Test3_SetupCalledTwoTimesAndTearDownCalledTwice()
    {
        Assume.That(_secondTestHasBeenRan, Is.True);
        //Arrange, Act & Assert
        var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _timesContextSetupHasBeenCalled;
        var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _timesContextTeardownHasBeenCalled;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.EqualTo(2));
            Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.EqualTo(2));
        }

    }
}
