namespace Api;

public class AppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
    public bool FeatureToggle { get; set; }
    public string ApplicationName { get; set; }
    public int MaxUsers { get; set; }
    public string Secret { get; set; }
}

public class ConnectionStrings
{
    public required string DefaultConnection { get; set; }
}
