
using KernelApi;

namespace Api;
/// <summary>
/// AppSettings specific to your application, inherits from  <see cref="BaseAppSettings"/>
/// </summary>
public class AppSettings : BaseAppSettings
{
    public bool FeatureToggle { get; set; }
}
