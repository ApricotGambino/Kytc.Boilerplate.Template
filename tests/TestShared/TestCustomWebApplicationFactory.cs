
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
    public TestCustomWebApplicationFactory(string environmentName)
    {
        if (string.IsNullOrWhiteSpace(environmentName))
        {
            throw new ArgumentNullException(environmentName, "An environment name must be provided when creating this class to avoid unexpected functionality.");
        }
        EnvironmentName = environmentName;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //This method hooks into the webhost build prior to actually being built.

        //We're using environmental variables to ensure that the correct appsettings.json is used during build.
        builder.UseEnvironment(EnvironmentName);

        //NOTE: This is called after the build completes it allows us to remove, or add services afterwards,
        //which is perfect for adding Test specific services that we don't normally want for our application.
        builder.ConfigureTestServices(services =>
        {
            services
                .AddDbContext<TestingDatabaseContext>();

            services.AddScoped<ITestExampleService, TestExampleService>();

        }
       );
    }
}




