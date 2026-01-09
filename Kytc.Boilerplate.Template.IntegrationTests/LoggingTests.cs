namespace Kytc.Boilerplate.Template.IntegrationTests;

using Domain.Entities.Admin;
using Kytc.Boilerplate.Template.IntegrationTests.Configurations;
using NUnit.Framework;

public class LoggingTests : BaseTestFixture
{
    [Test]
    public async Task AddLogEntry1()
    {
        var before = await TestContext.CountAsync<Log>();


        await TestContext.AddAsync(new Log
        {
            Message = "Test Log Entry1",
            Level = "good",
            MessageTemplate = "goodgood"
        });

        var after = await TestContext.CountAsync<Log>();

        Serilog.Log.Logger.Fatal("Added log entry??");
        Serilog.Log.Logger.Information("Info");
        Serilog.Log.Logger.Warning("Warn");
        Serilog.Log.Logger.Error("Error");
        Serilog.Log.Logger.Verbose("Verbose");
        Serilog.Log.Logger.Debug("Debug");

        Assert.That(after, Is.EqualTo(before + 1));
    }

    [Test]
    public async Task AddLogEntry()
    {
        var before = await TestContext.CountAsync<Log>();


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

    [Test]
    public async Task AddLogEntry1()
    {
        var before = await TestContext.CountAsync<Log>();

        await TestContext.AddAsync(new Log
        {
            Message = "Test Log Entry2",
            Level = "bad",
            MessageTemplate = "badbad"
        });

        var after = await TestContext.CountAsync<Log>();

        Assert.That(after, Is.EqualTo(before + 1));
    }
}
