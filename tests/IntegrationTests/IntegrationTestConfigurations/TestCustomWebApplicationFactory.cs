namespace IntegrationTests.IntegrationTestConfigurations;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;


/// <summary>
/// This is a custom web application factory, this calls our program.cs, but has the ability to modified outside normal execution that may be useful for running tests. 
/// </summary>
public class TestCustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public string _environmentName { get; private set; }
    public TestCustomWebApplicationFactory(string? environmentName = null)
    {
        if (environmentName != null)
        {
            this._environmentName = environmentName;
        }
        else
        {
            this._environmentName = "UnitTest";
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //This is called internally through .net's magic when this factory's asked to get services.
        //something like: customWebFactory.Services.GetRequiredService<IServiceScopeFactory>();

        //We're using environmental variables to ensure that the correct appsettings.json is used during build. 
        builder.UseEnvironment(this._environmentName);


        //This is intentionally left blank, but you can use this to override configuration of services after the build.
        //This could allow you to mock or replace services based on your needs for testing.
        //https://blog.markvincze.com/overriding-configuration-in-asp-net-core-integration-tests/
        builder.ConfigureTestServices(services => { });
    }
}




