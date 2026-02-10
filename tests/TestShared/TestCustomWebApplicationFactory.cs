namespace TestShared;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


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

        //This is called internally through .net's magic when this factory's asked to get services.
        //something like: customWebFactory.Services.GetRequiredService<IServiceScopeFactory>();

        //We're using environmental variables to ensure that the correct appsettings.json is used during build. 
        builder.UseEnvironment(EnvironmentName);

        //TODO: Explain this better, but evidnetly this gets called after the app is built,
        //and also you don't have to remove the old DB context, somehow adding a new one
        //just like...Overwrites it?
        builder.ConfigureTestServices(services =>
        {
            services
                //.RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<TestingDatabaseContext>();
        }
       );



        //This is intentionally left blank, but you can use this to override configuration of services after the build.
        //This could allow you to mock or replace services based on your needs for testing.
        //https://blog.markvincze.com/overriding-configuration-in-asp-net-core-integration-tests/
        //builder.ConfigureTestServices(services => { });
    }
}




