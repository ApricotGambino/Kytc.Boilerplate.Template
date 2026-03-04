using TestShared.Fixtures;

namespace Integration.Tests.ApiTests;

public class ApiRequestTests : SharedContextTestFixture
{
    [Test]
    public async Task ApiRequestTests1()
    {
        Assert.That(1 == 1);
    }
    [Test]
    public async Task ApiRequestTests2()
    {
        Assert.That(1 == 1);
    }
}
