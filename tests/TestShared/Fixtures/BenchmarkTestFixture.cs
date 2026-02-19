namespace TestShared.Fixtures;


/// <summary>
/// <inheritdoc />
/// Similar to <see cref="SharedContextTestFixture"/>, but intended to be used for benchmark testing
/// </summary>
[Category(TestingCategoryConstants.BenchmarkTests)]
public abstract class BenchmarkTestFixture : SharedContextTestFixture
{
    public override Task RunBeforeAnyTestsAsync()
    {
        return TestingContext.SetupTestContextAsync(TestingConstants.BenchmarkTestsEnvironmentName);
    }
}
