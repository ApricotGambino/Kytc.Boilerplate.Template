namespace KernelApi;
/// <summary>
/// This is the base app settings in which all the application must inherit from,
/// these are the configurations that the kernel depends on.
/// </summary>
public class BaseAppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
    public required string ApplicationName { get; set; }
    public int MaxUsers { get; set; }
    public required string Secret { get; set; }
    public required string Password { get; set; }
}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }

}
