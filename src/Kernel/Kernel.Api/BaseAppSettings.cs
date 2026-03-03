// BaseAppSettings.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

namespace Kernel.Api;
/// <summary>
/// This is the base app settings in which all the application must inherit from,
/// these are the configurations that the kernel depends on.
/// </summary>
public class BaseAppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
    public required string ApplicationName { get; set; }
    public required string ApplicationBaseUrl { get; set; }
    public Uri GetApplicationBaseUrlAsUri
    {
        get
        {
            return new Uri(ApplicationBaseUrl);
        }
    }
    public int MaxUsers { get; set; }
    public required string Secret { get; set; }
    public required string Password { get; set; }

    /// <summary>
    /// Configures if OpenAPI and Scalar are enabled.
    /// </summary>
    public bool EnableApiDiscovery { get; set; } = true;
}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }
}


