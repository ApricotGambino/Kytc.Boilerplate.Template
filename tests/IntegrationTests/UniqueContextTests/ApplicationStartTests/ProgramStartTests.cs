namespace IntegrationTests.UniqueContextTests.ApplicationStartTests;

using TestShared.Fixtures;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class ProgramStartTests : UniqueContextTestFixture
{
    [Test]
    public async Task StartupTest1()
    {
        Assert.Pass();
    }
    [Test]
    public async Task StartupTest2()
    {
        Assert.Pass();
    }
}
