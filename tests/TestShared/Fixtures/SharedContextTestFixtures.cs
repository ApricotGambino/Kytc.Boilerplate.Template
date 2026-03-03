using Api;

namespace TestShared.Fixtures;
/// <summary>
/// This nUnit test fixture is used for tests that use the database, and will share the database for all
/// tests. This fixture will create the database at the start of any tests ran, but will persist the database
/// throughout all tests hence the shared name.
/// </summary>
public abstract class SharedContextTestFixture : BaseTestFixture
{
    /// <summary>
    /// <inheritdoc />
    /// This will setup the context using <see cref="SetupTestContextAsync"/>
    /// </summary>
    /// <returns></returns>
    public override Task RunBeforeAnyTestsAsync()
    {
        return TestingContext.SetupTestContextAsync(TestingConstants.TestingEnvironmentName);
    }

    /// <summary>
    /// <inheritdoc />
    /// This will tear down the context using <see cref="TearDownTestContextAsync"/>
    /// </summary>
    /// <returns></returns>
    public override Task RunAfterAnyTestsAsync()
    {
        return TestingContext.TearDownTestContextAsync();
    }

    /// <summary>
    /// This will reset the context by providing custom AppSetting values for unit testing.
    /// </summary>
    /// <param name="appSettingConfigurationAction"></param>
    /// <param name="environmentName"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", "NUnit1028:The non-test method is public", Justification = "<Pending>")]
    public static Task ResetContextWithSpecificAppSettingAsync(Action<AppSettings>? appSettingConfigurationAction, string? environmentName = TestingConstants.TestingEnvironmentName)
    {
        return TestingContext.ResetTestContextAsync(environmentName, appSettingConfigurationAction);
    }
}

