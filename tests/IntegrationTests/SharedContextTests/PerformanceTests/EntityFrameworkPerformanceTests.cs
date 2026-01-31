namespace IntegrationTests.SharedContextTests.PerformanceTests;

using System.Reflection;
using Domain.Entities.Admin;
using Infrastructure.Data;
using TestShared.Fixtures;

public class EntityFrameworkPerformanceTests : SharedContextPerformanceTestFixture
{
    private double _averageElapsedTimesInMillisecondsForAddInLoop;
    private double _averageElapsedTimesInMillisecondsForAddAsyncInLoop;
    private double _averageElapsedTimesInMillisecondsForAddRange;
    private double _averageElapsedTimesInMillisecondsForAddRangeAsync;

    private bool _addAsyncInALoopRoughlyTheSameSpeedAsAddRangeAsyncHasRan;
    private bool _addInALoopRoughlyTheSameSpeedAsAddRangeHasRan;

    private const int _numberOfRecordsToCreate = 10;
    //NOTE: Increase this number to get more accurate results, but it will take longer to run.    
    //The higher the number, the less difference in percentage there will be, 100 runs vs 1000 runs is actually ~10x slower. 
    //But values at 100 you MAY run into percentage variances close to 5%, which could cause test failures, and it just depends on outliers. 
    private const int _numberTimesToRunTest = 100;
    [OneTimeSetUp]
    public virtual async Task MakeNugetPackageVersionAssumptions()
    {
        //NOTE: These test findings were found using the following two packages, 
        //so if those packages are updated, let's assume these tests are inconclusive
        //until this setup method is updated to the new versions of the packages.  Which can be found
        //in the src/Directory.Package.props. 


        //Microsoft.AspNetCore.Identity.EntityFrameworkCore Version="10.0.2.0"
        var efCoreVersion = Assembly.GetAssembly(typeof(Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext))!.GetName().Version!.ToString();
        //Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.2.0" />
#pragma warning disable EF1001 // Internal EF Core API usage.
        var sqlVersion = Assembly.GetAssembly(typeof(Microsoft.EntityFrameworkCore.SqlServer.Internal.SqlServerResources))!.GetName().Version!.ToString();
#pragma warning restore EF1001 // Internal EF Core API usage.

        Assume.That(efCoreVersion, Is.EqualTo("10.0.2.0"));
        Assume.That(sqlVersion, Is.EqualTo("10.0.2.0"));
    }

    private static List<Log> CreateLogsToInsert(int numberOfLogsToCreate)
    {
        var random = new Random();
        var listOfLogs = new List<Log>();

        for (var i = 0; i < numberOfLogsToCreate; i++)
        {
            listOfLogs.Add(new Log
            {
                Message = "EF Performance Test",
                Level = "Information",
                MessageTemplate = $"{random.Next(0, numberOfLogsToCreate)}"
            });
        }
        return listOfLogs;
    }
    private static async Task<long> InsertRecordsUsingAddRangeAndGetElapsedTime()
    {
        var dbContext = TestingContext.GetService<ApplicationDbContext>();
        var watch = System.Diagnostics.Stopwatch.StartNew();
        dbContext.AddRange(CreateLogsToInsert(_numberOfRecordsToCreate));
        await dbContext.SaveChangesAsync();
        watch.Stop();
        return watch.ElapsedMilliseconds;
    }
    private static async Task<long> InsertRecordsUsingAddRangeAsyncAndGetElapsedTime()
    {
        var dbContext = TestingContext.GetService<ApplicationDbContext>();
        var watch = System.Diagnostics.Stopwatch.StartNew();
        await dbContext.AddRangeAsync(CreateLogsToInsert(_numberOfRecordsToCreate));
        await dbContext.SaveChangesAsync();
        watch.Stop();
        return watch.ElapsedMilliseconds;
    }
    private static async Task<long> InsertRecordsUsingAddInLoopAndGetElapsedTime()
    {
        var dbContext = TestingContext.GetService<ApplicationDbContext>();
        var watch = System.Diagnostics.Stopwatch.StartNew();
        foreach (var log in CreateLogsToInsert(_numberOfRecordsToCreate))
        {
            dbContext.Add(log);
        }
        await dbContext.SaveChangesAsync();
        watch.Stop();
        return watch.ElapsedMilliseconds;
    }
    private static async Task<long> InsertRecordsUsingAddAsyncInLoopAndGetElapsedTime()
    {
        var dbContext = TestingContext.GetService<ApplicationDbContext>();
        var watch = System.Diagnostics.Stopwatch.StartNew();
        foreach (var log in CreateLogsToInsert(_numberOfRecordsToCreate))
        {
            await dbContext.AddAsync(log);
        }
        await dbContext.SaveChangesAsync();
        watch.Stop();
        return watch.ElapsedMilliseconds;
    }

    [Order(1)]
    [Test]
    public async Task AddAsyncInALoopRoughlyTheSameSpeedAsAddRangeAsync()
    {
        //NOTE: As of these package version: 
        //Microsoft.AspNetCore.Identity.EntityFrameworkCore Version="10.0.2.0"
        //Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.2.0" />
        //I assumed that AddRangeAsync would be faster than AddAsync in a loop,
        //since AddRangeAsync should avoid the constant change tracking (according to documentation)
        //however, that is not strictly true.  If you run these tests 10 times, you may find that 
        //Adding in a loop is usually faster, but it's not for sure, instead we just know they are very close.
        //This test may change as the packages update, but it is interesting and unintuitive. 

        //Arrange
        var elapsedTimesInMillisecondsForAddAsyncInLoop = new List<long>();
        var elapsedTimesInMillisecondsForAddRangeAsync = new List<long>();

        //Act
        for (var i = 0; i < _numberTimesToRunTest; i++)
        {
            elapsedTimesInMillisecondsForAddAsyncInLoop.Add(await InsertRecordsUsingAddAsyncInLoopAndGetElapsedTime());
            elapsedTimesInMillisecondsForAddRangeAsync.Add(await InsertRecordsUsingAddRangeAsyncAndGetElapsedTime());
        }

        _averageElapsedTimesInMillisecondsForAddAsyncInLoop = elapsedTimesInMillisecondsForAddAsyncInLoop.Average();
        _averageElapsedTimesInMillisecondsForAddRangeAsync = elapsedTimesInMillisecondsForAddRangeAsync.Average();

        var percentDifferenceInSpeedBetweenApproaches = 100.0;
        if (_averageElapsedTimesInMillisecondsForAddRangeAsync > 0)
        {
            percentDifferenceInSpeedBetweenApproaches = Math.Abs((100 * _averageElapsedTimesInMillisecondsForAddAsyncInLoop / _averageElapsedTimesInMillisecondsForAddRangeAsync) - 100);
        }

        //Assert
        Console.WriteLine($"The average addAsync in loop took: {_averageElapsedTimesInMillisecondsForAddAsyncInLoop}ms");
        Console.WriteLine($"The average addRangeAsync took: {_averageElapsedTimesInMillisecondsForAddRangeAsync}ms");
        Console.WriteLine($"The difference between the two was {percentDifferenceInSpeedBetweenApproaches:0.##}%");
        _addAsyncInALoopRoughlyTheSameSpeedAsAddRangeAsyncHasRan = true;
        Assert.That(percentDifferenceInSpeedBetweenApproaches, Is.LessThan(15), "Even the approach differences has different times, we really don't expect more than a 15% difference.  You may need to run this again, as it could be 'on your machine' at the time of running.");

    }

    [Order(2)]
    [Test]
    public async Task AddInALoopRoughlyTheSameSpeedAsAddRange()
    {
        //NOTE: As of these package version: 
        //Microsoft.AspNetCore.Identity.EntityFrameworkCore Version="10.0.2.0"
        //Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.2.0" />
        //I assumed that AddRange would be faster than Add in a loop,
        //since AddRange should avoid the constant change tracking (according to documentation)
        //however, that is not strictly true.  If you run these tests 10 times, you may find that 
        //Adding in a loop is usually faster, but it's not for sure, instead we just know they are very close.
        //This test may change as the packages update, but it is interesting and unintuitive. 

        //Arrange
        var elapsedTimesInMillisecondsForAddInLoop = new List<long>();
        var elapsedTimesInMillisecondsForAddRange = new List<long>();

        //Act
        for (var i = 0; i < _numberTimesToRunTest; i++)
        {
            elapsedTimesInMillisecondsForAddInLoop.Add(await InsertRecordsUsingAddInLoopAndGetElapsedTime());
            elapsedTimesInMillisecondsForAddRange.Add(await InsertRecordsUsingAddRangeAndGetElapsedTime());
        }

        _averageElapsedTimesInMillisecondsForAddInLoop = elapsedTimesInMillisecondsForAddInLoop.Average();
        _averageElapsedTimesInMillisecondsForAddRange = elapsedTimesInMillisecondsForAddRange.Average();

        var percentDifferenceInSpeedBetweenApproaches = 100.0;
        if (_averageElapsedTimesInMillisecondsForAddRangeAsync > 0)
        {
            percentDifferenceInSpeedBetweenApproaches = Math.Abs((100 * _averageElapsedTimesInMillisecondsForAddInLoop / _averageElapsedTimesInMillisecondsForAddRange) - 100);
        }

        //Assert
        Console.WriteLine($"The average add in loop took: {_averageElapsedTimesInMillisecondsForAddInLoop}ms");
        Console.WriteLine($"The average addRange took: {_averageElapsedTimesInMillisecondsForAddRange}ms");
        Console.WriteLine($"The difference between the two was {percentDifferenceInSpeedBetweenApproaches:0.##}%");
        _addInALoopRoughlyTheSameSpeedAsAddRangeHasRan = true;
        Assert.That(percentDifferenceInSpeedBetweenApproaches, Is.LessThan(15), "Even the approach differences has different times, we really don't expect more than a 15% difference.  You may need to run this again, as it could be 'on your machine' at the time of running.");

    }

    [Order(3)]
    [Test]
    public async Task AsyncAddMethodsAreFasterThanNonAsyncMethods()
    {
        if (_addAsyncInALoopRoughlyTheSameSpeedAsAddRangeAsyncHasRan && _addInALoopRoughlyTheSameSpeedAsAddRangeHasRan)
        {
            var percentDifferenceInSpeedBetweenAddInLoopApproaches = Math.Abs((100 * _averageElapsedTimesInMillisecondsForAddInLoop / _averageElapsedTimesInMillisecondsForAddAsyncInLoop) - 100);
            var percentDifferenceInSpeedBetweenAddRangeApproaches = Math.Abs((100 * _averageElapsedTimesInMillisecondsForAddRange / _averageElapsedTimesInMillisecondsForAddRangeAsync) - 100);
            Console.WriteLine($"The average add in loop took: {_averageElapsedTimesInMillisecondsForAddInLoop}ms");
            Console.WriteLine($"The average addAsync in loop took: {_averageElapsedTimesInMillisecondsForAddAsyncInLoop}ms");
            Console.WriteLine($"For adding in loop, the difference between Async and Non-Async is {percentDifferenceInSpeedBetweenAddInLoopApproaches:0.##}%");
            Console.WriteLine($"The average addRange took: {_averageElapsedTimesInMillisecondsForAddRange}ms");
            Console.WriteLine($"The average addRangeAsync took: {_averageElapsedTimesInMillisecondsForAddRangeAsync}ms");
            Console.WriteLine($"For adding with AddRange, the difference between Async and Non-Async is {percentDifferenceInSpeedBetweenAddRangeApproaches:0.##}%");


            using (Assert.EnterMultipleScope())
            {
                Assert.That(_averageElapsedTimesInMillisecondsForAddAsyncInLoop, Is.LessThan(_averageElapsedTimesInMillisecondsForAddInLoop));
                Assert.That(_averageElapsedTimesInMillisecondsForAddRangeAsync, Is.LessThan(_averageElapsedTimesInMillisecondsForAddRange));
            }
        }
        else
        {
            Assert.Inconclusive($"This test must be ran after {nameof(AddAsyncInALoopRoughlyTheSameSpeedAsAddRangeAsync)} and {nameof(AddInALoopRoughlyTheSameSpeedAsAddRange)}.");
        }


    }
}
