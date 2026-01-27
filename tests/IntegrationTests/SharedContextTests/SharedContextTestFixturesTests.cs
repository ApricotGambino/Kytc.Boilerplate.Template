//namespace IntegrationTests.SharedContextTests;


////This test verifies that the SharedContextTestFixturesTests actually creates a new context each time.
////Who tests the tests?  This test.
//[Category("nUnitFrameworkTests")]
//public class SharedContextTestFixturesTests : BaseTestFixture
//{
//    private const string _environmentNameUsedInFirstUnitTest = $"The Environment Name for the test context was applied in :{nameof(BaseSetUpFixtureOneTimeSetUp_ResetContext_HasSpecificEnvironmentName)}";
//    private bool _firstTestHasBeenRan;

//    [Order(1)]
//    [Test]
//    public async Task BaseSetUpFixtureOneTimeSetUp_ResetContext_HasSpecificEnvironmentName()
//    {
//        _firstTestHasBeenRan = true;
//        Assert.That(TestingContext._environmentName, Is.EqualTo(TestingConstants.EnvironmentName));
//        await TestingContext.ResetTestContext(_environmentNameUsedInFirstUnitTest);
//        Assert.That(TestingContext._environmentName, Is.EqualTo(_environmentNameUsedInFirstUnitTest));
//    }

//    [Order(2)]
//    [Test]
//    public async Task BaseSetUpFixtureOneTimeSetUp_DoNothing_HasSpecificEnvironmentNameSetFromFirstTest()
//    {
//        if (_firstTestHasBeenRan)
//        {
//            Assert.That(TestingContext._environmentName, Is.EqualTo(_environmentNameUsedInFirstUnitTest));
//        }
//        else
//        {
//            Assert.Inconclusive($"This test must be ran immediately after {nameof(BaseSetUpFixtureOneTimeSetUp_ResetContext_HasSpecificEnvironmentName)}.");
//        }
//    }
//}
