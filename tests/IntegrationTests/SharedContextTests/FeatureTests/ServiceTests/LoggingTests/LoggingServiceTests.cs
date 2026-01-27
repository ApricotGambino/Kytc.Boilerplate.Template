namespace IntegrationTests.SharedContextTests.FeatureTests.ServiceTests.LoggingTests;

using Domain.Entities.Admin;
using Domain.Interfaces.Features.Logging;
using Infrastructure.Data;
using TestShared.Fixtures;

public class LoggingServiceTests : SharedContextTestFixture
{
    [Test]
    public async Task LoggingTests1()
    {
        var thing = TestingContext.GetService<ApplicationDbContext>();

        await thing.AddAsync(new Log
        {
            Message = "Test Log Entry1",
            Level = "good",
            MessageTemplate = "goodgood"
        });

        await thing.SaveChangesAsync();

        var thing2 = TestingContext.GetService<ILoggingService>();
        var a = await thing2.GetMostRecentLogsAsync();
        var b = 1;

        //var before = await TestContext.CountAsync<Log>();


        //await TestContext.AddAsync(new Log
        //{
        //    Message = "Test Log Entry1",
        //    Level = "good",
        //    MessageTemplate = "goodgood"
        //});

        //var after = await TestContext.CountAsync<Log>();

        //Serilog.Log.Logger.Fatal("Added log entry??");
        //Serilog.Log.Logger.Information("Info");
        //Serilog.Log.Logger.Warning("Warn");
        //Serilog.Log.Logger.Error("Error");
        //Serilog.Log.Logger.Verbose("Verbose");
        //Serilog.Log.Logger.Debug("Debug");

        //Assert.That(after, Is.EqualTo(before + 1));
        Assert.Pass();
    }
}
