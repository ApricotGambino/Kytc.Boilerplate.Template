namespace Api;

public class AppSettings
{
    public bool FeatureToggle { get; set; }
    public string ApplicationName { get; set; }
    public int MaxUsers { get; set; }
    public string Secret { get; set; }
}
