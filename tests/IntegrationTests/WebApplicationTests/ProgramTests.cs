namespace IntegrationTests.WebApplicationTests;

using Domain.Entities.Admin;
using IntegrationTests.IntegrationTestConfigurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public class ProgramTests : BaseTestFixture
{
    //NOTE:TODO UPDATE THIS UPDATE
    //There's not a lot of unit testing you can, nor should do for the Program.cs file.
    //This file/class is considered a Composition Root (https://blog.ploeh.dk/2011/07/28/CompositionRoot/)
    //Most of what this does is just call other methods (which should be tested),
    //but we can test that our idea of the state of the application is as expected.
    //Also, we're not using the testing context intentionally because we want to test the actual unmodified web application builder.



    [Test]
    public async Task AddLogEntry()
    {
        var before = await TestContext.CountAsync<Log>();

        var client = TestContext.GetTestCustomWebApplicationFactory().CreateClient();

        // Act
        var response = await client.GetAsync("/appSettings");


        await TestContext.AddAsync(new Log
        {
            Message = "Test Log Entry1",
            Level = "good",
            MessageTemplate = "goodgood"
        });

        var after = await TestContext.CountAsync<Log>();

        Serilog.Log.Logger.Fatal("Added log entry??");

        Assert.That(after, Is.EqualTo(before + 1));
    }
}


[Category("ConfigurationTests")]
public class WebApplicationBuilderConfigurationsTests
{
    [Test]
    public void AddAppSettingsJsonFile_WhenEnvironmentIsUnitTest_LoadsUnitTestAppsettings()
    {
        using var factory = new TestCustomWebApplicationFactory(); // defaults to "UnitTest"
        var configuration = factory.Services.GetService<IConfiguration>();
        Assert.That(configuration, Is.Not.Null);

        var defaultConnection = configuration!.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value;

        Assert.That(defaultConnection, Is.EqualTo(
            @"Server=(localdb)\mssqllocaldb;Database=Kytc.Boilerplate.Template.UnitTest;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }

    [Test]
    public void AddAppSettingsJsonFile_WhenEnvironmentIsLocal_UsesBaseAppsettingsOnly()
    {
        using var factory = new TestCustomWebApplicationFactory(environmentName: "Local");
        var configuration = factory.Services.GetService<IConfiguration>();
        Assert.That(configuration, Is.Not.Null);

        var defaultConnection = configuration!.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value;

        Assert.That(defaultConnection, Is.EqualTo(
            @"Server=(localdb)\mssqllocaldb;Database=Kytc.Boilerplate.Template;Trusted_Connection=True;MultipleActiveResultSets=true"));
    }
}
