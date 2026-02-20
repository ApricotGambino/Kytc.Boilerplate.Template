namespace Sandbox.Concept_Demos.HowDoIGetData;

using KernelInfrastructure.Repositories;
using TestShared.Fixtures;
using TestShared.TestObjects;


//TODO: Make sure to do some pagination and stuff with .include and many-to-many

/// <summary>
/// These tests just show you different ways you can fetch data from the database, and how you SHOULD do it.
/// </summary>
[Category(TestingCategoryConstants.SandboxTests)]
public class HowDoIGetData : SharedContextTestFixture
{
    private List<TestEntity> CreatedEntities { get; set; }

    [OneTimeSetUp]
    public async Task SeedTestData()
    {
        CreatedEntities = await TestingContext.InsertTestEntitiesIntoDatabaseAsync(10);
    }

    [Test]
    [Order(1)]
    public async Task Get_Data_From_DB_Context()
    {
        //You are able to get data directly from the dbContext, and there may be times this is what you need to do.
        //This is very common, and how it's often done, let me show you how:

        //NOTE: Here we're fetching the DB from the Service Provider in the testing context,
        //but normally you'd get this from DI injection into a constructor into a class like a service.
        var dbContext = TestingContext.GetTestingDatabaseContext();

        //From here, we can fetch the data as usual:
        var fetchedData = dbContext.TestEntities.ToList();

        //Just as a sanity check, we just want to make sure that the data we fetched contains all the elements of the ones we seeded.
        Assert.That(CreatedEntities.Select(s => s.Id), Is.SubsetOf(fetchedData.Select(s => s.Id)));
    }

    [Test]
    [Order(2)]
    public async Task Get_Data_From_ReadOnly_Repo()
    {
        //You are able to get data directly from the ReadOnly Repository,
        //this is different than how you may be used to, but it comes with some slight performance advantages.
        //We'll discuss this later on, but for now, just as a proof of concept we'll just prove
        //this way gets data pretty much the same as directly from the DB context.

        //The repo is an injected service like the DBcontext, we just have to specify the entity we are fetching.
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        //This really doing very little different than directly from the context,
        //it's only really including .AsNoTracking() in the background and returning the queryable.
        //Also, take note of the 'WARNING' in the remarks of the method.
        var fetchedData = readonlyRepo.GetEntityQueryable().ToList();

        //Just as a sanity check, we just want to make sure that the data we fetched contains all the elements of the ones we seeded.
        Assert.That(CreatedEntities.Select(s => s.Id), Is.SubsetOf(fetchedData.Select(s => s.Id)));
    }

    [Test]
    [Order(3)]
    public async Task Get_Data_From_Service()
    {
        //Getting data from the DBContext, or the ReadonlyRepo are really the only two ways you can fetch
        //data, but it's pretty common that you'll get data coming from a service.
        //In fact, I'd argue that almost every time you fetch from the DB, you're going to probably
        //in a service already, as there's very few times you should access the DB outside of that
        //abstraction layer, by which I mean, you should never be hitting the DB context directly
        //in something like a controller method.

        //Much like before, this service is injected.
        var testExampleService = TestingContext.GetService<ITestExampleService>();

        //This example service fetches data in both ways, ReadOnly repo, and also directly from the DB context.
        var serviceDataFromContext = await testExampleService.GetAllEntitiesUsingContextAsync();
        var serviceDataFromReadOnlyRepo = await testExampleService.GetAllEntitiesUsingReadOnlyRepoAsync();

        //So far, so boring.  But, we can see how data can move through layers, and up through abstractions.
        //DBContext -> ReadOnlyRepo -> Service

        //Now we'll just prove that the return of the two methods of the service are the exact same.
        Assert.That(serviceDataFromContext.Select(s => s.Id), Is.EqualTo(serviceDataFromReadOnlyRepo.Select(s => s.Id)));
    }

    [Test]
    [Order(3)]
    public async Task asdfasdf()
    {
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var fetchedData = readonlyRepo.TEST(p => p.ABool, 1, 2);
    }

    //TODO: Create methods of paginated repo.

    //Ok, so we know we can get data either directly from the DBContext, or the ReadOnlyRepo.
    //So then why use the repo?
    //Because this Repo usage is going to feel irritating to use, since it's always going to fight you on
    //giving you 'all the data'.  That's very intentional.
    //There's a benchmark test called 'EFCoreToListBenchmarks' that shows the performance between  the different ways you can 'get data'.
    //Here's a quick excerpt of those results, using 1000 records, and 'taking' 50 of them:
    //| EFCore_NoTake                   Records taken: 1000 | Time: 2,198.3 us   | Allocated Memory: 1664.32 KB
    //| EFCore_WithTake                 Records taken: 50   | Time:   400.3 us   | Allocated Memory:  161.32 KB
    //| EFCore_WithTake_AndAsNoTracking Records taken: 50   | Time:   374.5 us   | Allocated Memory:  127.63 KB

    //These results shows us a few things:
    //1) It's faster to fetch a subset of data (in this case, 50 records) than to take all of them.
    //2) It takes less memory to take a smaller subset of data.
    //3) Including 'AsNoTracking' in our query in both speed and memory allocation is better on the whole, the only downside is you'd have to reattach the entity for changes to be commited to the DB.

    //This is obvious of course, naturally taking a subset of data will yield better performance,
    //and that's what this repo seeks to guide you to do.
    //By being opinionated about always asking you to consider how much data you're returning, you never have the chance to forget including pagination as part of your result set.

    //It is not always possible to paginate, and that's why you can use GetEntityQueryable() which at least comes with the small .AsNoTracking benfit.

    //When you SHOULD use pagination
    //Any time you're returning data to the client, because the client side should never recieve the entire table's worth of data. (Unless of course you have a very, VERY small table)
    //This covers...Almost every possible example, but not all.
    //EX:
    //If you need to get all users who are in a status of pending, and return them to the client:
    //users.where(status = pending).skip(10).take(10);

    //When you CANNOT use pagination:
    //Any time you MUST iterate over all the data.
    //EX:
    //If you need to get all users who are in a status of pending, and send them an email:
    //emailUserAboutStatus(users.where(status = pending))

    //Then there are times that aren't clear.
    //EX:
    //If you need to get all users who are in a status of pending, and send them an email, then return back to the client a list of all the users who you've just sent emails to.
    //So right off the bat this is a strange one, because is this return a one-and-done?
    //If the client that issued the request loses internet, do they just...Never get the chance to see what happened?
    //So already we can see we probably need to compose the code in a way that lends to being able to use pagination to some degree.
    //First, email the pending users, and have that email function update a record somewhere in the database to indicate an email was sent
    //emailUserAboutStatus(users.where(status = pending))
    //Then, fetch that updated list with pagination:
    //sentEmails.where(type = emailedUserAboutStatus).OrderBy(date).skip(10).take(10);


    //[Test]
    //public async Task Get_Data_From_DB_Context()
    //{
    //    var dbContext = TestingContext.GetTestingDatabaseContext();

    //    var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
    //    var testExampleService = TestingContext.GetService<ITestExampleService>();

    //    var a = await testExampleService.GetMostRecentEntitiesUsingContextAsync();
    //    var b = await testExampleService.GetMostRecentEntitiesUsingReadOnlyRepoAsync();

    //    Assert.Pass();
    //}
}
