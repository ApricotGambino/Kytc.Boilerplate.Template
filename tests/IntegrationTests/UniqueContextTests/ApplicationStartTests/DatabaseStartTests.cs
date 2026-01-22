namespace IntegrationTests.UniqueContextTests.ApplicationStartTests;

using TestShared.Fixtures;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class DatabaseStartTests : UniqueContextTestFixture
{
    //[Test]
    //public async Task DatabaseTests1()
    //{
    //    var a = 1;
    //}
    //[Test]
    //public async Task DatabaseTests2()
    //{
    //    var a = 1;
    //}
    private const string _environmentNameUsedInFirstUnitTest = $"The Environment Name for the test context was applied in :{nameof(UniqueContextTestTestFixtureTestSetUp_SetupContext_HasSpecificEnvironmentName)}";


    [Order(1)]
    [Test]
    public async Task UniqueContextTestTestFixtureTestSetUp_SetupContext_HasSpecificEnvironmentName()
    {
        await TestingContext.SetupTestContext(_environmentNameUsedInFirstUnitTest);
        Assert.That(TestingContext.EnvironmentName, Is.EqualTo(_environmentNameUsedInFirstUnitTest));
    }

    [Order(2)]
    [Test]
    public async Task UniqueContextTestTestFixtureTestSetUp_TestSetupHasToreDownContext_EnvironmentNameIsNotTheSameAsPriorTest()
    {
        Assert.That(TestingContext.EnvironmentName, Is.Not.EqualTo(_environmentNameUsedInFirstUnitTest));
    }
}
