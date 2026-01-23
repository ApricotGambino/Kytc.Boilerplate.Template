namespace IntegrationTests.UniqueContextTests.ApplicationStartTests;

using TestShared.Fixtures;

[Category(TestingCategoryConstants.ApiStartupTests)]
public class DatabaseStartTests : UniqueContextTestFixture
{
    [Order(1)]
    [Test]
    public async Task DatabaseStartTests1()
    {
        Assert.Pass();
    }

    [Order(2)]
    [Test]
    public async Task DatabaseStartTests2()
    {
        Assert.Pass();
    }
}
