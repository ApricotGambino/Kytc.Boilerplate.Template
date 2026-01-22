namespace Api;

public class AppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
    public bool FeatureToggle { get; set; }
    public required string ApplicationName { get; set; }
    public int MaxUsers { get; set; }
    public required string Secret { get; set; }
    public required string Password { get; set; }
}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }

}
