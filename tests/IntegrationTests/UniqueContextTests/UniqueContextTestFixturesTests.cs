//namespace IntegrationTests.UniqueContextTests.ApplicationStartTests;

////This test verifies that the UniqueContextTestTestFixture actually creates a new context each time.
////Who tests the tests?  This test.
//[Category("nUnitFrameworkTests")]
//public class UniqueContextTestFixturesTests : UniqueContextTestTestFixture
//{
//    private const string _environmentNameUsedInFirstUnitTest = $"The Environment Name for the test context was applied in :{nameof(UniqueContextTestTestFixtureTestSetUp_SetupContext_HasSpecificEnvironmentName)}";


//    [Order(1)]
//    [Test]
//    public async Task UniqueContextTestTestFixtureTestSetUp_SetupContext_HasSpecificEnvironmentName()
//    {
//        await TestingContext.SetupTestContext(_environmentNameUsedInFirstUnitTest);
//        Assert.That(TestingContext._environmentName, Is.EqualTo(_environmentNameUsedInFirstUnitTest));
//    }

//    [Order(2)]
//    [Test]
//    public async Task UniqueContextTestTestFixtureTestSetUp_TestSetupHasToreDownContext_EnvironmentNameIsNotTheSameAsPriorTest()
//    {
//        Assert.That(TestingContext._environmentName, Is.Not.EqualTo(_environmentNameUsedInFirstUnitTest));
//    }
//}
