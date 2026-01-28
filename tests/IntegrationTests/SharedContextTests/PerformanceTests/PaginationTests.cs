namespace IntegrationTests.SharedContextTests.FeatureTests.ServiceTests.LoggingTests;

using Domain.Entities.Admin;
using Infrastructure.Data;
using TestShared.Fixtures;

public class PaginationTests : SharedContextPerformanceTestFixture
{

    //[Test]
    //public async Task _start()
    //{
    //    Random random = new Random();
    //    int randomNumber = random.Next(0, 1000);
    //    var thing = TestingContext.GetService<ApplicationDbContext>();
    //    var listOfLogs = new List<Log>();

    //    for (int i = 0; i < 10000; i++)
    //    {
    //        listOfLogs.Add(new Log
    //        {
    //            Message = $"Test Log Entry{i}",
    //            Level = "Information",
    //            MessageTemplate = $"{randomNumber}"
    //        });
    //    }

    //    foreach (var log in listOfLogs)
    //    {
    //        await thing.AddAsync(log);
    //    }

    //    await thing.SaveChangesAsync();
    //    Assert.Pass();
    //}
    [Test]
    public async Task PerformanceTestThing()
    {
        var thing = TestingContext.GetService<ApplicationDbContext>();
        var listOfLogs = new List<Log>();

        for (int i = 0; i < 10000; i++)
        {
            listOfLogs.Add(new Log
            {
                Message = $"Test Log Entry{i}",
                Level = "Information",
                MessageTemplate = ""
            });
        }

        await thing.AddRangeAsync(listOfLogs);

        foreach (var log in listOfLogs)
        {
            await thing.AddAsync(log);
        }

        await thing.AddRangeAsync(listOfLogs);

        await thing.SaveChangesAsync();

        //var thing2 = TestingContext.GetService<ILoggingService>();
        //var a = await thing2.GetMostRecentLogsAsync();
        //var b = 1;

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

    [Test]
    public async Task b()
    {
        Random random = new Random();
        int randomNumber = random.Next(0, 10000);
        var thing = TestingContext.GetService<ApplicationDbContext>();
        var listOfLogs = new List<Log>();

        for (int i = 0; i < 10000; i++)
        {
            listOfLogs.Add(new Log
            {
                Message = $"Test Log Entry{i}",
                Level = "Information",
                MessageTemplate = $"{randomNumber}"
            });
        }
        var watch = System.Diagnostics.Stopwatch.StartNew();
        await thing.AddRangeAsync(listOfLogs);

        await thing.SaveChangesAsync();
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Assert.Pass();
    }

    [Test]
    public async Task a()
    {
        Random random = new Random();
        int randomNumber = random.Next(0, 10000);
        var thing = TestingContext.GetService<ApplicationDbContext>();
        var listOfLogs = new List<Log>();

        for (int i = 0; i < 10000; i++)
        {
            listOfLogs.Add(new Log
            {
                Message = $"Test Log Entry{i}",
                Level = "Information",
                MessageTemplate = $"{randomNumber}"
            });
        }

        foreach (var log in listOfLogs)
        {
            await thing.AddAsync(log);
        }

        await thing.SaveChangesAsync();
        Assert.Pass();
    }
}
