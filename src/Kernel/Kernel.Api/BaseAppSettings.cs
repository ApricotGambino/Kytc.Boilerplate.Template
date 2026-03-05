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
    /// <summary>
    /// This is the name of the application
    /// </summary>
    public required string ApplicationName { get; set; }
    /// <summary>
    /// This is the base URL of the application ///
    /// </summary>
    /// <remarks>EX: www.website.com</remarks>
    public required string ApplicationBaseUrl { get; set; }
    /// <summary>
    /// This returns a properly formatted Uri object of the <see cref="ApplicationBaseUrl"/>
    /// </summary>
    public Uri GetApplicationBaseUrlAsUri
    {
        get
        {
            return new Uri(ApplicationBaseUrl);
        }
    }
    /// <summary>
    /// Configures if OpenAPI and Scalar are enabled.
    /// </summary>
    public bool EnableApiDiscovery { get; set; } = true;

    /// <summary>
    /// Configures if the application should perform an Entity Framework migration automatically on start.
    /// </summary>
    public bool EnableAutomaticMigrations { get; set; } = true;


    public int MaxUsers { get; set; }
    public required string Secret { get; set; }
    public required string Password { get; set; }


}

public class ConnectionStrings
{
    /// <summary>
    /// This is the connection string used for the database.
    /// </summary>
    public required string DefaultConnection { get; set; }
}


