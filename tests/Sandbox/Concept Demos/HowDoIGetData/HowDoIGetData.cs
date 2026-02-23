
using KernelData.Extensions.Pagination;
using KernelInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using TestShared.Fixtures;
using TestShared.TestObjects;

namespace Sandbox.Concept_Demos.HowDoIGetData;
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
        //Resetting the context just in case any other tests influence these results.
        await TestingContext.ResetTestContextAsync();
        CreatedEntities = await TestingContext.InsertTestEntitiesIntoDatabaseAsync(10);
        await SchoolEntityHelper.InsertExampleSchoolSetup();
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
        var fetchedData = await dbContext.TestEntities.ToListAsync();

        //Just as a sanity check, we just want to make sure that the data we fetched contains all the elements of the ones we seeded.
        Assert.That(CreatedEntities.Select(s => s.Id), Is.SubsetOf(fetchedData.Select(s => s.Id)));
    }

    [Test]
    [Order(2)]
    public async Task Get_Data_From_ReadOnly_Repo_Using_The_Queryable()
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
        var fetchedData = await readonlyRepo.GetEntityQueryable().ToListAsync();

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

        //Moving forward, we're going to use the repo with pagination, because that's really what you should do, you'll save yourself
        //possible performance woes by doing so.
    }

    [Test]
    public async Task Get_Data_From_ReadOnly_Repo_Oldest_Records_First()
    {
        //From here on out, we're going to only be using the repo, and that means only doing it with paging.
        //These examples are what you'd
        //put in your service methods, and we're going to show the repo methods being used.
        //As you've already seen from Get_Data_From_ReadOnly_Repo_Using_The_Queryable, you
        //can always just get the queryable if you need, and do whatever you want from there,
        //but using the specific methods for fetching data will give you the benefits of pagination
        //and performance.

        //In this example, we're fetching the oldest records.
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var pageSize = 2;

        var fetchedData = await readonlyRepo.GetPaginatedEntityOrderedByIdOldestFirstAsync(1, pageSize);
        var fetchedDataIds = fetchedData.Results!.Select(s => s.Id);
        var recordIdsFromTheDatabaseOrderedById = await dbContext.TestEntities.OrderBy(o => o.Id).Take(pageSize).Select(s => s.Id).ToListAsync();
        var recordIdsFromTheDatabaseOrderedByCreatedDate = await dbContext.TestEntities.OrderBy(o => o.CreatedDateTimeOffset).Take(pageSize).Select(s => s.Id).ToListAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataIds, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedByCreatedDate, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedById, Is.EqualTo(recordIdsFromTheDatabaseOrderedByCreatedDate));
            Assert.That(fetchedDataIds, Is.EqualTo(recordIdsFromTheDatabaseOrderedById));
        }
    }

    [Test]
    public async Task Get_Data_From_ReadOnly_Repo_Newest_Records_First()
    {
        //In this example, we're fetching the newest records.
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var pageSize = 2;

        var fetchedData = await readonlyRepo.GetPaginatedEntityOrderedByIdNewestFirstAsync(1, pageSize);
        var fetchedDataIds = fetchedData.Results.Select(s => s.Id);
        var recordIdsFromTheDatabaseOrderedById = await dbContext.TestEntities.OrderByDescending(o => o.Id).Take(pageSize).Select(s => s.Id).ToListAsync();
        var recordIdsFromTheDatabaseOrderedByCreatedDate = await dbContext.TestEntities.OrderByDescending(o => o.CreatedDateTimeOffset).Take(pageSize).Select(s => s.Id).ToListAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataIds, Is.Ordered.Descending);
            Assert.That(recordIdsFromTheDatabaseOrderedByCreatedDate, Is.Ordered.Descending);
            Assert.That(recordIdsFromTheDatabaseOrderedById, Is.EqualTo(recordIdsFromTheDatabaseOrderedByCreatedDate));
            Assert.That(fetchedDataIds, Is.EqualTo(recordIdsFromTheDatabaseOrderedById));
        }
    }

    [Test]
    public async Task Get_Data_From_ReadOnly_Repo_OrderedBy_AString_Ascending()
    {
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var pageSize = 2;

        var fetchedData = await readonlyRepo.GetPaginatedEntityAscendingAsync(1, pageSize, o => o.AString);
        var fetchedDataStrings = fetchedData.Results.Select(s => s.AString);
        var recordIdsFromTheDatabaseOrderedByAString = await dbContext.TestEntities.OrderBy(o => o.AString).Take(pageSize).Select(s => s.AString).ToListAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataStrings, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedByAString, Is.Ordered.Ascending);
            Assert.That(fetchedDataStrings, Is.EqualTo(recordIdsFromTheDatabaseOrderedByAString));
        }
    }

    [Test]
    public async Task Get_Data_From_ReadOnly_Repo_OrderedBy_AString_And_Filtered_By_Only_False_Values_For_ABool_Descending()
    {
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();
        var pageSize = 2;

        var fetchedData = await readonlyRepo.GetPaginatedEntityAscendingAsync(1, pageSize, o => o.AString, p => !p.ABool);
        var fetchedDataStrings = fetchedData.Results.Select(s => s.AString);
        var fetchedDataBools = fetchedData.Results.Select(s => s.ABool);
        var recordIdsFromTheDatabaseOrderedByAString = await dbContext.TestEntities.Where(p => !p.ABool).OrderBy(o => o.AString).Take(pageSize).Select(s => s.AString).ToListAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(fetchedDataBools.Distinct().First(), Is.False); //Ensure all the records we fetched are false, as we would expect.
            Assert.That(fetchedDataStrings, Is.Ordered.Ascending);
            Assert.That(recordIdsFromTheDatabaseOrderedByAString, Is.Ordered.Ascending);
            Assert.That(fetchedDataStrings, Is.EqualTo(recordIdsFromTheDatabaseOrderedByAString));
        }
    }

    [Test]
    public async Task Get_Data_Only_One_Record_Without_Pagination_From_ReadOnly_Repo()
    {
        //So not every time we need a list of results, but we may still want to take advantage of the no tracking benefits
        //of the repo.  We could do this using GetEntityQueryable, but there are helper method we could use instead.

        var dbContext = TestingContext.GetTestingDatabaseContext();
        var readonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<TestEntity, TestingDatabaseContext>>();

        var firstTestEntity = await dbContext.TestEntities.FirstAsync();

        //Get by Id, if we're doing this, we KNOW the record exists.
        var singleById = await readonlyRepo.GetSingleEntityByIdAsync(firstTestEntity.Id);

        //Get by where clause
        var firstOrDefaultResult = await readonlyRepo.GetFirstOrDefaultEntityByFilterAsync(p => p.AStringWithNumbers == firstTestEntity.AStringWithNumbers);

        var firstOrDefaultResultThatDoesntExist = await readonlyRepo.GetFirstOrDefaultEntityByFilterAsync(p => p.AStringWithNumbers == "This should not exist");


        using (Assert.EnterMultipleScope())
        {
            Assert.That(singleById.Id, Is.EqualTo(firstTestEntity.Id));
            Assert.That(firstOrDefaultResult, Is.Not.Null);
            Assert.That(firstOrDefaultResult!.Id, Is.EqualTo(firstTestEntity.Id));
            Assert.That(firstOrDefaultResultThatDoesntExist, Is.Null);

            //If we try to use GetSingleEntityByIdAsync on something not found, like id = -1, then we should throw an error.
            Assert.ThrowsAsync<InvalidOperationException>(() => readonlyRepo.GetSingleEntityByIdAsync(-1));
        }
    }

    [Test]
    public async Task Get_All_Bimmy_The_Student_Courses_Using_Queryable()
    {
        //In these tests, we'll use the established 'School' data.
        // In our setup, Bimmy has three courses, English, History and Math.
        var studentReadonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<Student, TestingDatabaseContext>>();

        var studentQueryable = studentReadonlyRepo.GetEntityQueryable();

        var studentMissingPopulatedCourses = await studentQueryable.Where(p => p.Name == "Bimmy").FirstAsync();
        //By fetching this Student entity, you'll notice the Courses property is null, that's because we didn't do an include.
        Assert.That(studentMissingPopulatedCourses.Courses, Is.Null);

        var studentIncludingCourses = await studentQueryable.Include(i => i.Courses).Where(p => p.Name == "Bimmy").FirstAsync();
        Assert.That(studentIncludingCourses.Courses, Is.Not.Null);
        Assert.That(studentIncludingCourses.Courses.Select(s => s.Name), Is.EquivalentTo(["English", "Math", "History"]));

        //So as you can see, this all works, but there's no pagination. Let's do an example with pagination.
    }

    [Test]
    public async Task Get_Student_Courses_Using_ToPaginatedResultsAsync()
    {
        //We've seen how we can get data using the repo for connected entities in Get_All_Bimmy_The_Student_Courses_Using_Queryable(),
        //But if we include pagination, what does that mean?

        //Let's say I want to display a table of students, and in that grid, I want to list all the courses they  have.
        //In our example, we only have 3 students, but let's say we have a page size of 2, meaning there should be two pages
        //of results, in the first page, we'll show two students, and in the second page, just one.
        //EX:
        //Page 1:
        //Adam - English
        //Jimmy - History, Math
        //Page 2:
        //Bimmy - English, History, Math.
        //This makes sense, and feels pretty intuitive.

        //But if you were strictly thinking in terms of pagination being a way to limit the amount of data
        //we're returning, then it's very unclear.
        //Because in the above example, we're fetching pages of students, but the returned student object has ALL
        //the courses, so consider if Bimmy had 1,000,000 courses? Silly of course, but technically possible.


        //This is why we'll assume that included entities through navigation properties returns everything.
        //Fortunately the inclduded entities are inherently filtered by the FK to the primary entity,
        //so we're not grabbing the whole table, but it's something you should be thinking about.

        //For the above reasons, we'll use the GetEntityQueryable() so we can add our .Include() or even .ThenInclude()s
        //and then use the pagination extension method to turn that into a paginated result.

        var pageSize = 2;
        var studentReadonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<Student, TestingDatabaseContext>>();
        var studentQueryable = studentReadonlyRepo.GetEntityQueryable().Include(i => i.Courses);
        var dbContext = TestingContext.GetTestingDatabaseContext();

        var allStudents = await dbContext.Students.ToListAsync();

        var firstPageOfStudents = await studentQueryable.ToPaginatedResultsAsync(1, pageSize);
        var secondPageOfStudents = await studentQueryable.ToPaginatedResultsAsync(2, pageSize);
        var thirdPageOfStudents = await studentQueryable.ToPaginatedResultsAsync(3, pageSize);

        var firstStudentOfFirstPage = firstPageOfStudents.Results![0];
        var firstStudentOfFirstPageCourses = await dbContext.StudentToCourses.Where(p => p.StudentId == firstStudentOfFirstPage.Id).Select(s => s.Course).ToListAsync();


        Assume.That(allStudents.Count == 3);
        using (Assert.EnterMultipleScope())
        {
            //Make sure that the first page has two students.
            Assert.That(firstPageOfStudents.Page, Is.EqualTo(1));
            Assert.That(firstPageOfStudents.Results, Has.Exactly(pageSize).Items);
            Assert.That(firstPageOfStudents.TotalItems, Is.EqualTo(allStudents.Count));
            Assert.That(firstPageOfStudents.TotalPages, Is.EqualTo((int)Math.Ceiling(allStudents.Count / (double)pageSize)));

            //Make sure the second page of results has one student.
            Assert.That(secondPageOfStudents.Page, Is.EqualTo(2));
            Assert.That(secondPageOfStudents.Results, Has.Exactly(allStudents.Count - firstPageOfStudents.PageSize).Items);
            Assert.That(secondPageOfStudents.TotalItems, Is.EqualTo(allStudents.Count));
            Assert.That(secondPageOfStudents.TotalPages, Is.EqualTo((int)Math.Ceiling(allStudents.Count / (double)pageSize)));

            //Make sure the third page doesn't actually have any data.
            Assert.That(thirdPageOfStudents.Page, Is.EqualTo(3));
            Assert.That(thirdPageOfStudents.Results, Is.Empty);
            Assert.That(secondPageOfStudents.TotalItems, Is.EqualTo(allStudents.Count));
            Assert.That(thirdPageOfStudents.TotalPages, Is.EqualTo(2));

            //Finally, let's make sure that the first student, on the first page had all the courses included as expected:
            Assert.That(firstStudentOfFirstPage.Courses.Select(s => s.Id), Is.EqualTo(firstStudentOfFirstPageCourses.Select(s => s.Id)));
        }
    }

    [Test]
    public async Task Get_Student_Courses_Paginated_Results_Without_Using_ToPaginatedResultsAsync()
    {
        //Before we had to use GetEntityQueryable() in junction with ToPaginatedResultsAsync() to use .Include()
        //There is a way to do that without the need of getting a queryable, and then remembering to return with paginated results.

        //We can include the entity name as part of the parameter for either GetPaginatedEntityAscendingAsync, or GetPaginatedEntityDescendingAsync
        //This works by finding the navigation property by name.
        //Normally you'd do:
        //context.entity.include(i => i.navigationProperty)
        //But EF will let you do this:
        //context.entity.include("entity.navigationProperty")
        //You can TECHNICALLY do multi level includes, like you'd normally use '.thenInclude()' but I don't recommend it,
        //because it gets pretty messy pretty quick, if you need to do multiple includes, go back to theGetEntityQueryable() in junction with ToPaginatedResultsAsync() strategy.

        var pageSize = 2;
        var dbContext = TestingContext.GetTestingDatabaseContext();
        var studentReadonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<Student, TestingDatabaseContext>>();


        var firstPageOfStudentsWithCourses = await studentReadonlyRepo.GetPaginatedEntityAscendingAsync(1, pageSize, order: o => o.Id, where: null, includeEntityName: nameof(Student.Courses));

        var firstStudentOfFirstPage = firstPageOfStudentsWithCourses.Results.First();
        var firstStudentOfFirstPageCourses = await dbContext.StudentToCourses.Where(p => p.StudentId == firstStudentOfFirstPage.Id).Select(s => s.Course).ToListAsync();

        Assert.That(firstStudentOfFirstPage.Courses.Select(s => s.Id), Is.EqualTo(firstStudentOfFirstPageCourses.Select(s => s.Id)));
    }

    //[Test]
    //public async Task Get_All_Bimmy_The_Student_Courses()
    //{
    //    //In these tests, we'll use the established 'School' data.
    //    // In our setup, Bimmy has three courses, English, History and Math.
    //    var studentReadonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<Student, TestingDatabaseContext>>();
    //    var courseReadonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<Course, TestingDatabaseContext>>();
    //    var studentToCourseReadonlyRepo = TestingContext.GetService<ReadOnlyEntityRepo<StudentToCourse, TestingDatabaseContext>>();

    //    var bimmy = await studentReadonlyRepo.GetFirstOrDefaultEntityByFilterAsync(p => p.Name == "Bimmy");
    //    var allStudentToCourseByBimmyId = await studentToCourseReadonlyRepo.GetPaginatedEntityAscendingAsync(pageNumber: 1, pageSize: 100, p => p.S);

    //    Assert.Pass();
    //    //var adam = db.Students.Where(p => p.Name == "Adam").First();

    //    //Assert.That(adam.Courses, Has.Count.EqualTo(1));
    //    //Assert.That(adam.Courses.First().Name, Is.EqualTo("English"));
    //}

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
