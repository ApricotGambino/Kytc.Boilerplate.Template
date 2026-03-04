using TestShared.Fixtures;

namespace Integration.Tests.InfrastructureTests;

public class ExceptionTests : SharedContextTestFixture
{
    [Test]
    public async Task ExceptionTests1()
    {
        Assert.That(1 == 1);
    }
    [Test]
    public async Task ExceptionTests2()
    {
        Assert.That(1 == 1);
    }
}
