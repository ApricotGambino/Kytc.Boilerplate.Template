namespace Sandbox;

using Domain.Entities.Admin;
using Domain.Interfaces.Features.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestShared.Fixtures;

public class TDDSandboxTests : SharedContextTestFixture
{
    private static List<Log> CreateLogsToInsert(int numberOfLogsToCreate, string callingMethodName)
    {
        var random = new Random();
        var listOfLogs = new List<Log>();
        var listOfLevels = new List<string>() { nameof(LogLevel.Critical), nameof(LogLevel.Information), nameof(LogLevel.Warning), nameof(LogLevel.Error) };

        for (var i = 0; i < numberOfLogsToCreate; i++)
        {
            var log = new Log
            {
                Message = $"Logging Service Test number {i}, created by {callingMethodName}",
                Level = listOfLevels.OrderBy(o => Guid.NewGuid()).First(),
                MessageTemplate = $"{random.Next(0, numberOfLogsToCreate)}"
            };

            log.SetCreatedDate(DateTimeOffset.Now.AddDays(i - numberOfLogsToCreate));

            listOfLogs.Add(log);
        }
        return listOfLogs;
    }
    private const int _numberOfLogsToCreate = 20;

    [Test]
    public async Task GetMostRecentLogsAsync_CreateRecordsWithKnownDates_ReturnsExpectedResults()
    {
        var loggingService = TestingContext.GetService<ILoggingService>();
        //var dbContext = TestingContext.GetService<ApplicationDbContext>();
        var expected = CreateLogsToInsert(_numberOfLogsToCreate, nameof(GetMostRecentLogsAsync_CreateRecordsWithKnownDates_ReturnsExpectedResults));
        await SeedRangeAsync(expected);

        var results = await loggingService.GetMostRecentLogsUsingContextAsync();

        Assert.That(results, Is.Ordered.Descending.By(nameof(Log.CreatedDateTimeOffset)));
    }

    //[Test]
    //public async Task GetMostRecentLogsAsync_CreateRecordsWithKnownDates_ReturnsExpectedResults()
    //{
    //    var loggingService = TestingContext.GetService<ILoggingService>();
    //    //var dbContext = TestingContext.GetService<ApplicationDbContext>();
    //    var expected = CreateLogsToInsert(_numberOfLogsToCreate, nameof(GetMostRecentLogsAsync_CreateRecordsWithKnownDates_ReturnsExpectedResults));
    //    await SeedRangeAsync(expected);

    //    var results = await loggingService.GetMostRecentLogsUsingContextAsync();

    //    Assert.That(results, Is.Ordered.Descending.By(nameof(Log.CreatedDateTimeOffset)));
    //}

    //[Test]
    //public async Task ListAsync_ReturnsAllItems()
    //{
    //    var expected = CreateLogsToInsert(_numberOfLogsToCreate, nameof(ListAsync_ReturnsAllItems));
    //    await SeedRangeAsync(expected);

    //    var repo = new Repository<Country>(DbContext);

    //    var result = await repo.ListAsync();

    //    result.Should().HaveSameCount(expected);
    //    result.Should().BeEquivalentTo(expected);
    //}
}
