namespace TestShared;

using KernelApi;

/// <summary>
/// AppSettings specific to your application, inherits from  <see cref="BaseAppSettings"/>
/// </summary>
public class TestAppSettings : BaseAppSettings
{
    public required string TestKey { get; set; }
}
