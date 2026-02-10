//namespace IntegrationTests.SharedContextTests.InfrastructureTests.RepositoryTests;

//using System;
//using System.Collections.Generic;
//using Domain.Entities.Admin;
//using Infrastructure.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using TestShared.Fixtures;
//using TestShared.TestObjects;

//public class ReadOnlyEntityRepoTests : SharedContextTestFixture
//{
//    private static List<Log> CreateLogsToInsert(int numberOfLogsToCreate, string callingMethodName)
//    {
//        var random = new Random();
//        var listOfLogs = new List<Log>();
//        var listOfLevels = new List<string>() { nameof(LogLevel.Critical), nameof(LogLevel.Information), nameof(LogLevel.Warning), nameof(LogLevel.Error) };

//        for (var i = 0; i < numberOfLogsToCreate; i++)
//        {
//            var log = new Log
//            {
//                Message = $"Logging Service Test number {i}, created by {callingMethodName}",
//                Level = listOfLevels.OrderBy(o => Guid.NewGuid()).First(),
//                MessageTemplate = $"{random.Next(0, numberOfLogsToCreate)}"
//            };

//            log.SetCreatedDate(DateTimeOffset.Now.AddDays(i - numberOfLogsToCreate));

//            listOfLogs.Add(log);
//        }
//        return listOfLogs;
//    }
//    private const int _numberOfLogsToCreate = 20;

//    [Test]
//    public async Task asdf()
//    {
//        var dbContext = TestingContext.GetService<TestingDatabaseContext>();
//        var readonlyContext = new ReadOnlyEntityRepo<TestEntity>(dbContext);
//        //var expected = CreateLogsToInsert(_numberOfLogsToCreate, nameof(asdf));
//        var expected = TestObjectUsingBaseEntityHelper.CreateTestObjectList(100);
//        await SeedRangeAsync(expected);

//        //var results = await loggingService.GetMostRecentLogsUsingContextAsync();
//        var dbContextResults = await dbContext.TestEntities.ToListAsync();
//        var readonlyContextResults = await readonlyContext.GetEntityQueryable().ToListAsync();

//        using (Assert.EnterMultipleScope())
//        {
//            Assert.That(expected.Select(s => s.Id), Is.SubsetOf(dbContextResults.Select(s => s.Id)));
//            Assert.That(expected.Select(s => s.Id), Is.SubsetOf(readonlyContextResults.Select(s => s.Id)));
//        }
//    }
//}
