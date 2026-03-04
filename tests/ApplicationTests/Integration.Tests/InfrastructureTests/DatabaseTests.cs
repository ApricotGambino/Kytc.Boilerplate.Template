using TestShared.Fixtures;

namespace Integration.Tests.InfrastructureTests;

public class DatabaseTests : SharedContextTestFixture
{
    [Test]
    public async Task DatabaseTests1()
    {
        Assert.That(1 == 1);
    }
    [Test]
    public async Task DatabaseTests2()
    {
        Assert.That(1 == 1);
    }
}
