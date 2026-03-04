using TestShared.Fixtures;

namespace Integration.Tests.InfrastructureTests;

public class LoggingTests : SharedContextTestFixture
{
    [Test]
    public async Task LoggingTests1()
    {
        Assert.That(1 == 1);
    }
    [Test]
    public async Task LoggingTests2()
    {
        Assert.That(1 == 1);
    }
}
