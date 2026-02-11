namespace TestingInfrastructureTests;

using TestShared;
using TestShared.Fixtures;

/// <summary>
/// These tests ensure that <see cref="SharedContextTestFixture"/> creates a TestingContext at the start,
///  and always shares the context unless intentionally reset.
/// </summary>
[Category(TestingCategoryConstants.NUnitFrameworkTests)]
public class SharedContextTestFixturesTest : SharedContextTestFixture
{
    private bool _FirstTestHasBeenRan;

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

        await TestingContext.SetupTestContextAsync(TestingConstants.TestingEnvironmentName);
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
        _FirstTestHasBeenRan = true;
    }

    [Order(2)]
    [Test]
    public async Task SharedContextTestFixtureTestSetUpTestSetUp_Test2DoNothing_EnvironmentNameIsStillTheSameAsPriorTest()
    {
        Assume.That(_FirstTestHasBeenRan, Is.True);

        //Arrange, Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }
}




/// <summary>
/// These tests ensure that <see cref="SharedContextTestFixture"></> Setup and Teardown calls are called the correct and expected number of times between runs.
///  and always shares the context unless intentionally reset.
/// </summary>
[Category(TestingCategoryConstants.NUnitFrameworkTests)]
public class SharedContextTestFixtureSetupAndTearDownTests : SharedContextTestFixture
{
    private int _TimesContextSetupHasBeenCalled;
    private int _TimesContextTeardownHasBeenCalled;
    private int _TimesContextResetHasBeenCalled;
    private bool _FirstTestHasBeenRan;
    private bool _SecondTestHasBeenRan;
    private bool _ThirdTestHasBeenRan;

    [Order(1)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test1_ContextShouldHaveBeenSetupAtLeastOnceByTheTimeThisTestIsRan()
    {
        //Arrange, Act & Assert
        _TimesContextSetupHasBeenCalled = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        _TimesContextTeardownHasBeenCalled = TestingContext.__metadata_NumberOfTearDownTestContextCalls;
        _TimesContextResetHasBeenCalled = TestingContext.__metadata_NumberOfResetTestContextCalls;

        Assert.That(TestingContext.__metadata_NumberOfSetupTestContextCalls, Is.GreaterThanOrEqualTo(1));
        _FirstTestHasBeenRan = true;
    }

    [Order(2)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test2_SetupAndTearDownShouldNotHaveBeenCalled()
    {
        Assume.That(_FirstTestHasBeenRan, Is.True);
        //Arrange, Act & Assert
        var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _TimesContextSetupHasBeenCalled;
        var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _TimesContextTeardownHasBeenCalled;
        var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - _TimesContextResetHasBeenCalled;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
            Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.Zero);
            Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.Zero);
            _SecondTestHasBeenRan = true;
        }
    }

    [Order(3)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test3_SetupAndTearDownStillShouldNotHaveBeenCalled()
    {
        Assume.That(_SecondTestHasBeenRan, Is.True);
        //NOTE: This test makes sure that nothing has changed between the first, and third test.
        //Arrange, Act & Assert
        var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _TimesContextSetupHasBeenCalled;
        var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _TimesContextTeardownHasBeenCalled;
        var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - _TimesContextResetHasBeenCalled;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.Zero);
            Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.Zero);
            Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.Zero);
            _ThirdTestHasBeenRan = true;
        }
    }
    [Order(4)]
    [Test]
    public async Task SharedContextTestFixtureSetupAndTearDownTests_Test4CallsReset_ResetAndSetupAndTeardownShouldHaveBeenCalledOnce()
    {
        Assume.That(_ThirdTestHasBeenRan, Is.True);
        //NOTE: ResetTest should call teardown and setup, along with the base test calling setup
        //Arrange, Act & Assert
        await TestingContext.ResetTestContextAsync(TestingContext.EnvironmentName);
        var numberOfTimesSetupHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfSetupTestContextCalls - _TimesContextSetupHasBeenCalled;
        var numberOfTimesTearDownHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _TimesContextTeardownHasBeenCalled;
        var numberOfTimesResetHasBeenCalledSinceFirstTest = TestingContext.__metadata_NumberOfResetTestContextCalls - _TimesContextResetHasBeenCalled;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(numberOfTimesSetupHasBeenCalledSinceFirstTest, Is.EqualTo(1));
            Assert.That(numberOfTimesTearDownHasBeenCalledSinceFirstTest, Is.EqualTo(1));
            Assert.That(numberOfTimesResetHasBeenCalledSinceFirstTest, Is.EqualTo(1));
        }
    }
}
