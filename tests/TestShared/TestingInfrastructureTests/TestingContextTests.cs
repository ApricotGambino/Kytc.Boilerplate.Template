namespace TestShared.TestingInfrastructureTests;
/// <summary>
/// NOTE: These tests ensure that TestingContext Setup/Teardown/Reset all work as expected.
/// Because the TestingContext is a static class, we need to make sure these actually work, and can't rely on letting the
/// class become null, or constructors to build it up as expected.  Also, it's static because it's funtionally a singleton
/// that can be shared throughout tests.
/// Also, these tests are the only ones that don't use any of the established nunit testing fixtures we've created.
/// We don't want those to influence anything.
/// If all these test pass, that means the testing context does the following:
/// 1) Setup populates the context completely.
/// 2) Teardown makes everything in the context null.
/// 3) Reset calls the Teardown, then the Setup.
/// This ensures that the context works as you'd expect throughout all the testing you man do.
/// </summary>
[Category(TestingCategoryConstants.NUnitFrameworkTests)]
[TestFixture]
public class TestingContextTests
{
    private int _numberOfTimesTheseTestsHaveCalledSetup;
    private int _numberOfTimesTheseTestsHaveCalledTeardown;
    private int _numberOfTimesTheseTestsHaveCalledReset;

    private int _metadataNumberOfSetupTestContextCallsBeforeTestsAreRan;
    private int _metadataNumberOfTearDownTestContextCallsBeforeTestsAreRan;
    private int _metadataNumberOfResetTestContextCallsBeforeTestsAreRan;

    private bool _stopTests;

    [OneTimeSetUp]
    public async Task RunBeforeTheseTests()
    {
        _metadataNumberOfSetupTestContextCallsBeforeTestsAreRan = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        _metadataNumberOfTearDownTestContextCallsBeforeTestsAreRan = TestingContext.__metadata_NumberOfTearDownTestContextCalls;
        _metadataNumberOfResetTestContextCallsBeforeTestsAreRan = TestingContext.__metadata_NumberOfResetTestContextCalls;

        await TestingContext.TearDownTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledTeardown++;
    }

    [TearDown]
    public async Task TestTearDown()
    {
        //We want to stop running tests if any have failed, since these tests rely on each one before it.
        if (TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Passed)
        {
            _stopTests = true;
        }

        Assume.That(_stopTests, Is.False);
    }

    [Test]
    [Order(1)]
    public async Task TestingContext_Test01ClassStructure_TestingContextCannotHaveFields()
    {
        //NOTE: This seems odd, but we don't want our testing class to have any fields, only properties.
        //This is so that we're able to ensure all 'fields' are actually 'properties' so that we can also force each one has a private setter.
        //so that tests can only interact with the context through Setup/Reset/Teardown methods.
        //I am _CERTAIN_ there's a better way to design this class to make this naturally the case, but instead we'll just throw errors in the test.
        var fields = typeof(TestingContext).GetFields();
        using (Assert.EnterMultipleScope())
        {
            foreach (var field in fields)
            {
                Assert.Fail($"The field '{field.Name}' was found in the Testing Context, you should convert that field to a property with a private setter instead.");
            }
        }
    }

    [Test]
    [Order(2)]
    public async Task TestingContext_Test02ClassStructure_TestingContextCannotPropertiesWithPublicSetters()
    {
        //NOTE: This seems odd, but we don't want our testing class to have any public setters,
        //so that tests can only interact with the context through Setup/Reset/Teardown methods.
        //I am _CERTAIN_ there's a better way to design this class to make this naturally the case, but instead we'll just throw errors in the test.
        var properties = typeof(TestingContext).GetProperties().Where(p => p.GetMethod != null && p.SetMethod != null && p.GetMethod.IsPublic && p.SetMethod.IsPublic);
        using (Assert.EnterMultipleScope())
        {
            foreach (var property in properties)
            {
                Assert.Fail($"The property '{property.Name}' was found in the Testing Context with a public setter, you should make the setter private instead.");
            }
        }
    }

    [Test]
    [Order(3)]
    public async Task TestingContext_Test03SetupContext_ContextHasDefaultEnvironmentName()
    {
        //Arrange
        await TestingContext.SetupTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledSetup++;

        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }

    [Test]
    [Order(4)]
    public async Task TestingContext_Test04DoNothing_ContextStillHasDefaultEnvironmentName()
    {
        //Arrange, Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }

    [Test]
    [Order(5)]
    public async Task TestingContext_Test05AttemptToSetupContextAgainWithoutTeardown_ThrowsError()
    {
        //Arrange, Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => TestingContext.SetupTestContextAsync());
    }

    [Test]
    [Order(6)]
    public async Task TestingContext_Test06TearsDownContext_ContextHasDefaultEnvironmentName()
    {
        //Arrange
        await TestingContext.TearDownTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledTeardown++;

        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }

    [Test]
    [Order(7)]
#pragma warning disable S4144 // Methods should not have identical implementations
    public async Task TestingContext_Test07SetupContextAfterTeardown_ContextSetupShouldNotThrowError()
#pragma warning restore S4144 // Methods should not have identical implementations
    {
        //Arrange
        await TestingContext.SetupTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledSetup++;

        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }

    [Test]
    [Order(8)]
    public async Task TestingContext_Test08ResetContext_ContextShouldBeResetWithAlreadyInitializedContext()
    {
        //Arrange
        await TestingContext.ResetTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledReset++;

        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }

    [Test]
    [Order(9)]
    public async Task TestingContext_Test09TearDownContextPriorToResetting_ContextShouldBeResetEvenIfTheContextIsToreDown()
    {
        //Arrange
        await TestingContext.TearDownTestContextAsync();
        await TestingContext.ResetTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledTeardown++;
        _numberOfTimesTheseTestsHaveCalledReset++;

        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }
    [Test]
    [Order(10)]
    public async Task TestingContext_Test10SetupContextWithSpecificEnvironmentName_ContextHasSpecificEnvironmentName()
    {
        //Arrange
        await TestingContext.TearDownTestContextAsync();
        await TestingContext.SetupTestContextAsync(nameof(TestingContext_Test10SetupContextWithSpecificEnvironmentName_ContextHasSpecificEnvironmentName));
        _numberOfTimesTheseTestsHaveCalledTeardown++;
        _numberOfTimesTheseTestsHaveCalledSetup++;

        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(nameof(TestingContext_Test10SetupContextWithSpecificEnvironmentName_ContextHasSpecificEnvironmentName)));
    }

    [Test]
    [Order(11)]
    public async Task TestingContext_Test11DoNothing_ContextStillHasSpecificEnvironmentName()
    {
        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(nameof(TestingContext_Test10SetupContextWithSpecificEnvironmentName_ContextHasSpecificEnvironmentName)));
    }

    [Test]
    [Order(12)]
    public async Task TestingContext_Test12Resets_ContextHasDefaultEnvironmentName()
    {
        //Act & Assert
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(nameof(TestingContext_Test10SetupContextWithSpecificEnvironmentName_ContextHasSpecificEnvironmentName)));
        await TestingContext.ResetTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledReset++;
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(TestingConstants.TestingEnvironmentName));
    }

    [Test]
    [Order(13)]
    public async Task TestingContext_Test13ResetCallsSetupAndTeardown_TeardownAndSetupMetavaluesIncrementByOneAndAreNotReset()
    {
        //Act
        var setupCountBeforeReset = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        var teardownCountBeforeReset = TestingContext.__metadata_NumberOfTearDownTestContextCalls;
        await TestingContext.ResetTestContextAsync();
        _numberOfTimesTheseTestsHaveCalledReset++;
        var setupCountAfterReset = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        var teardownCountAfterReset = TestingContext.__metadata_NumberOfTearDownTestContextCalls;
        using (Assert.EnterMultipleScope())
        {
            //Assert
            Assert.That(setupCountAfterReset - setupCountBeforeReset, Is.EqualTo(1));
            Assert.That(teardownCountAfterReset - teardownCountBeforeReset, Is.EqualTo(1));
        }
    }

    [Test]
    [Order(14)]
    public async Task TestingContext_Test14ContextAlreadySetupButCallSetupAgainToError_MetadataSetupCallsShouldNotIncrementFromError()
    {
        //Arrange, Act & Assert
        //NOTE: At this point the context should still be active, so setting it up again should fail.
        var setupCallsPriorToError = TestingContext.__metadata_NumberOfSetupTestContextCalls;
        Assert.ThrowsAsync<InvalidOperationException>(() => TestingContext.SetupTestContextAsync());
        Assert.That(setupCallsPriorToError, Is.EqualTo(TestingContext.__metadata_NumberOfSetupTestContextCalls));

    }

    [Test]
    [Order(15)]
    public async Task TestingContext_Test15CompareNumberOfTimesMethodsWereCalledInCodeToTestSuiteCounter_CallCountsMatch()
    {
        //First, we'll check our own work by setting these values
        //to what we believe it should be, by having only read the code (using ctrl+f for the method).

        //Setup was called technically 5 times, but two of them was an intentional error,
        //so we didn't really initialize, and won't count those, and because of Test: TestingContext_Test14ContextAlreadySetupButCallSetupAgainToError_MetadataSetupCallsShouldNotIncrementFromError,
        //we know the context won't count it either.
        const int numberOfTimesSetupTestContextWasSuccessfullyCalledInthisTestSuite = 3;
        const int numberOfTimesTearDownTestContextMethodWasCalledInthisTestSuite = 4;
        const int numberOfTimesResetMethodWasCalledInthisTestSuite = 4;
        using (Assert.EnterMultipleScope())
        {
            //This tells us that the number of times we found calls to these methods is the same as the number
            //of times we incremented the count in this suite. This test isn't great, but it's a sanity check.
            Assert.That(_numberOfTimesTheseTestsHaveCalledSetup, Is.EqualTo(numberOfTimesSetupTestContextWasSuccessfullyCalledInthisTestSuite));
            Assert.That(_numberOfTimesTheseTestsHaveCalledTeardown, Is.EqualTo(numberOfTimesTearDownTestContextMethodWasCalledInthisTestSuite));
            Assert.That(_numberOfTimesTheseTestsHaveCalledReset, Is.EqualTo(numberOfTimesResetMethodWasCalledInthisTestSuite));
        }
    }

    [Test]
    [Order(16)]
    public async Task TestingContext_Test16TestSuiteCounterToMetaDataCounts_CallCountsMatch()
    {
        //NOTE: Each 'reset' will call Setup and Teardown once, so we count those in the final count.
        var numberOfTimesResetMethodWasCalledInthisTestSuite = _numberOfTimesTheseTestsHaveCalledReset;
        var numberOfTimesTearDownTestContextMethodWasCalledInthisTestSuite = _numberOfTimesTheseTestsHaveCalledReset + _numberOfTimesTheseTestsHaveCalledTeardown;
        var numberOfTimesSetupTestContextWasSuccessfullyCalledInthisTestSuite = _numberOfTimesTheseTestsHaveCalledReset + _numberOfTimesTheseTestsHaveCalledSetup;

        //Because this test suite may be ran after other tests, we want to only get the delta of metadata counts since this suite has been ran.
        var metadataResetDelta = TestingContext.__metadata_NumberOfResetTestContextCalls - _metadataNumberOfResetTestContextCallsBeforeTestsAreRan;
        var metadataTeardownDelta = TestingContext.__metadata_NumberOfTearDownTestContextCalls - _metadataNumberOfTearDownTestContextCallsBeforeTestsAreRan;
        var metadataStartDelta = TestingContext.__metadata_NumberOfSetupTestContextCalls - _metadataNumberOfSetupTestContextCallsBeforeTestsAreRan;

        using (Assert.EnterMultipleScope())
        {
            //Assert
            Assert.That(numberOfTimesResetMethodWasCalledInthisTestSuite, Is.EqualTo(metadataResetDelta));
            Assert.That(numberOfTimesTearDownTestContextMethodWasCalledInthisTestSuite, Is.EqualTo(metadataTeardownDelta));
            Assert.That(numberOfTimesSetupTestContextWasSuccessfullyCalledInthisTestSuite, Is.EqualTo(metadataStartDelta));
        }
    }
}




