//namespace IntegrationTests.SharedContextTests.FeatureTests.ServiceTests.LoggingTests;

//using System;
//using System.Collections.Generic;
//using Domain.Entities.Admin;
//using Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using TestShared.Fixtures;

//public class PaginationTests : SharedContextPerformanceTestFixture
//{
//    private const int _numberOfRecordsToCreate = 10000;
//    private const int _pageSize = 10;


//    private static List<Log> CreateLogsToInsert(int numberOfLogsToCreate)
//    {
//        var random = new Random();
//        var listOfLogs = new List<Log>();

//        for (var i = 0; i < numberOfLogsToCreate; i++)
//        {
//            listOfLogs.Add(new Log
//            {
//                Message = "EF Performance Test",
//                Level = "Information",
//                MessageTemplate = $"{random.Next(0, numberOfLogsToCreate)}"
//            });
//        }
//        return listOfLogs;
//    }


//    [OneTimeSetUp]
//    public virtual async Task InsertLogRecordsForPerformanceTesting()
//    {
//        var random = new Random();
//        var listOfLogs = new List<Log>();
//        var dbContext = TestingContext.GetService<ApplicationDbContext>();

//        for (var i = 1; i <= _numberOfRecordsToCreate; i++)
//        {
//            listOfLogs.Add(new Log
//            {
//                Message = $"This is the Pagination Performance Test record number {i}",
//                Level = "Pagination Performance Test",
//                MessageTemplate = $"{random.Next(0, _numberOfRecordsToCreate)}"
//            });
//        }

//        await dbContext.AddRangeAsync(listOfLogs);
//        await dbContext.SaveChangesAsync();
//    }

//    [Test]
//    public async Task asdfasdf()
//    {
//        var dbContext = TestingContext.GetService<ApplicationDbContext>();
//        var allLogsCount = await dbContext.Logs.CountAsync();


//        //var a = dbContext.Logs.Where(p => p.Level == "Pagination Performance Test").ToList();
//        //var d = await dbContext.Logs.Where(p => p.Level == "Pagination Performance Test").ToListAsync();
//        ////var c = dbContext.Logs.OffsetPaginationDBSETTEST(o => o.Message, 1, 10);
//        //var cc = dbContext.Logs.OffsetPaginationDBSETTEST(o => o.Message, 1, 10);
//        //var ccc = await dbContext.Logs.OffsetPaginationDBSETTESTAsync(o => o.Message, 1, 10);

//        //var pageOfDataKeyset = dbContext.Logs.OffsetPaginationDBSETTEST(o => o.Id, pageNumber: 1, pageSize: _pageSize);
//        //var pageOfDataOffset = dbContext.Logs.OffsetPaginationDBSETTEST(o => o.Id, pageNumber: 1, pageSize: _pageSize);
//        //var keysetIteerations = 0;
//        //var offsetIteerations = 0;


//        var watch = System.Diagnostics.Stopwatch.StartNew();
//        var pageOfDataOffset = dbContext.Logs.OffsetPaginationDBSETTEST(o => o.Id, pageNumber: 50, pageSize: _pageSize);
//        //for (var i = 1; i < allLogsCount / _pageSize; i++)
//        //{
//        //    if (pageOfDataKeyset.Count != 0)
//        //    {
//        //        pageOfDataKeyset = dbContext.Logs.GetNextPageUsingKeysetPaginationasdfasdfasdf(o => o.Id, lastValue: pageOfDataKeyset.Last().Id, lastId: pageOfDataKeyset.Last().Id, _pageSize);
//        //        keysetIteerations++;
//        //    }
//        //}
//        watch.Stop();

//        var watch2 = System.Diagnostics.Stopwatch.StartNew();
//        var pageOfDataKeyset = dbContext.Logs.GetNextPageUsingKeysetPaginationasdfasdfasdf(o => o.Id, lastValue: pageOfDataOffset.Last().Id, lastId: pageOfDataOffset.Last().Id, _pageSize);
//        //for (var i = 1; i < allLogsCount / _pageSize; i++)
//        //{
//        //    if (pageOfDataOffset.Count != 0)
//        //    {
//        //        pageOfDataOffset = dbContext.Logs.OffsetPaginationDBSETTEST(o => o.Id, pageNumber: i, pageSize: _pageSize);
//        //        offsetIteerations++;
//        //    }
//        //}
//        watch2.Stop();




//        var diff = watch.ElapsedMilliseconds - watch2.ElapsedMilliseconds;
//        var asdfasd2 = 1;
//    }

//    //[Test]
//    //public async Task _start()
//    //{
//    //    Random random = new Random();
//    //    int randomNumber = random.Next(0, 1000);
//    //    var thing = TestingContext.GetService<ApplicationDbContext>();
//    //    var listOfLogs = new List<Log>();

//    //    for (int i = 0; i < 10000; i++)
//    //    {
//    //        listOfLogs.Add(new Log
//    //        {
//    //            Message = $"Test Log Entry{i}",
//    //            Level = "Information",
//    //            MessageTemplate = $"{randomNumber}"
//    //        });
//    //    }

//    //    foreach (var log in listOfLogs)
//    //    {
//    //        await thing.AddAsync(log);
//    //    }

//    //    await thing.SaveChangesAsync();
//    //    Assert.Pass();
//    //}
//    //[Test]
//    //public async Task PerformanceTestThing()
//    //{
//    //    var thing = TestingContext.GetService<ApplicationDbContext>();
//    //    var listOfLogs = new List<Log>();

//    //    for (int i = 0; i < 10000; i++)
//    //    {
//    //        listOfLogs.Add(new Log
//    //        {
//    //            Message = $"Test Log Entry{i}",
//    //            Level = "Information",
//    //            MessageTemplate = ""
//    //        });
//    //    }

//    //    await thing.AddRangeAsync(listOfLogs);

//    //    foreach (var log in listOfLogs)
//    //    {
//    //        await thing.AddAsync(log);
//    //    }

//    //    await thing.AddRangeAsync(listOfLogs);

//    //    await thing.SaveChangesAsync();

//    //    //var thing2 = TestingContext.GetService<ILoggingService>();
//    //    //var a = await thing2.GetMostRecentLogsAsync();
//    //    //var b = 1;

//    //    //var before = await TestContext.CountAsync<Log>();


//    //    //await TestContext.AddAsync(new Log
//    //    //{
//    //    //    Message = "Test Log Entry1",
//    //    //    Level = "good",
//    //    //    MessageTemplate = "goodgood"
//    //    //});

//    //    //var after = await TestContext.CountAsync<Log>();

//    //    //Serilog.Log.Logger.Fatal("Added log entry??");
//    //    //Serilog.Log.Logger.Information("Info");
//    //    //Serilog.Log.Logger.Warning("Warn");
//    //    //Serilog.Log.Logger.Error("Error");
//    //    //Serilog.Log.Logger.Verbose("Verbose");
//    //    //Serilog.Log.Logger.Debug("Debug");

//    //    //Assert.That(after, Is.EqualTo(before + 1));
//    //    Assert.Pass();
//    //}

//    //[Test]
//    //public async Task b()
//    //{
//    //    Random random = new Random();
//    //    int randomNumber = random.Next(0, 10000);
//    //    var thing = TestingContext.GetService<ApplicationDbContext>();
//    //    var listOfLogs = new List<Log>();

//    //    for (int i = 0; i < 10000; i++)
//    //    {
//    //        listOfLogs.Add(new Log
//    //        {
//    //            Message = $"Test Log Entry{i}",
//    //            Level = "Information",
//    //            MessageTemplate = $"{randomNumber}"
//    //        });
//    //    }
//    //    var watch = System.Diagnostics.Stopwatch.StartNew();
//    //    await thing.AddRangeAsync(listOfLogs);

//    //    await thing.SaveChangesAsync();
//    //    watch.Stop();
//    //    var elapsedMs = watch.ElapsedMilliseconds;
//    //    Assert.Pass();
//    //}

//    //[Test]
//    //public async Task a()
//    //{
//    //    Random random = new Random();
//    //    int randomNumber = random.Next(0, 10000);
//    //    var thing = TestingContext.GetService<ApplicationDbContext>();
//    //    var listOfLogs = new List<Log>();

//    //    for (int i = 0; i < 10000; i++)
//    //    {
//    //        listOfLogs.Add(new Log
//    //        {
//    //            Message = $"Test Log Entry{i}",
//    //            Level = "Information",
//    //            MessageTemplate = $"{randomNumber}"
//    //        });
//    //    }

//    //    foreach (var log in listOfLogs)
//    //    {
//    //        await thing.AddAsync(log);
//    //    }

//    //    await thing.SaveChangesAsync();
//    //    Assert.Pass();
//    //}
//}
