using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TestShared.TestObjects;

namespace TestShared;
/// <summary>
/// This is a custom web application factory, this calls our program.cs, but has the ability to modified outside normal execution that may be useful for running tests.
/// </summary>
public class TestCustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public string EnvironmentName { get; }
    /// <summary>
    /// This is a lambda that represents the values to be provided for the AppSetting, so that the values can be
    /// overridden in test cases instead of relying on the actual appsetting.json file.
    /// </summary>
    public Action<AppSettings>? AppSettingConfigurationAction { get; }
    public TestCustomWebApplicationFactory(string environmentName, Action<AppSettings>? appSettingConfigurationAction = null)
    {
        if (string.IsNullOrWhiteSpace(environmentName))
        {
            throw new ArgumentNullException(environmentName, "An environment name must be provided when creating this class to avoid unexpected functionality.");
        }
        EnvironmentName = environmentName;
        AppSettingConfigurationAction = appSettingConfigurationAction;
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //This method is executed prior to anything in program.cs.

        //We're using environmental variables to ensure that the correct appsettings.json is used during build.
        builder.UseEnvironment(EnvironmentName);

        //NOTE: This is called immediately after builder.Build(); in program.cs it allows us to remove, or add services,
        //which is perfect for adding Test specific services that we don't normally want for our application.
        builder.ConfigureTestServices(services =>
        {
            if (AppSettingConfigurationAction != null)
            {
                services.Configure<AppSettings>(AppSettingConfigurationAction);
            }

            services
                .AddDbContext<TestingDatabaseContext>();

            services.AddScoped<ITestExampleService, TestExampleService>();
        }
       );
    }
}




