
using TestShared.Fixtures;
using TestShared.TestObjects;

namespace Sandbox.Concept_Demos.EFRelationship;
/// <summary>
/// This is showcasing a working example of the ef-relationships concept in the documentation. We're not using the Readonly Repo for this example
/// to not conflate concepts, this is just inserting entities and fetching them directly from the DB context to showcase relationships.
/// This setup data is used in HowDoIGetData.cs as well, that showcases fetching data using the readonly repo.
/// </summary>
[Category(TestingCategoryConstants.SandboxTests)]
public class EFRelationship : SharedContextTestFixture
{

    [OneTimeSetUp]
    public async Task SeedTestData()
    {
        //Resetting the context just in case any other tests influence these results.
        await TestingContext.ResetTestContextAsync();
        await SchoolEntityHelper.InsertExampleSchoolSetup();
    }


    [Test]
    public async Task Get_All_Adams_Courses()
    {
        var db = TestingContext.GetTestingDatabaseContext();

        var adam = db.Students.Where(p => p.Name == "Adam").First();

        Assert.That(adam.Courses, Has.Count.EqualTo(1));
        Assert.That(adam.Courses.First().Name, Is.EqualTo("English"));
    }

    [Test]
    public async Task Get_All_English_Students()
    {
        var db = TestingContext.GetTestingDatabaseContext();

        var english = db.Courses.Where(p => p.Name == "English").First();

        Assert.That(english.Students, Has.Count.EqualTo(2));
        Assert.That(english.Students.ConvertAll(s => s.Name), Is.EqualTo(["Adam", "Bimmy"]));
    }

    [Test]
    public async Task Get_All_Courses_MrBusy_Teaches()
    {
        var db = TestingContext.GetTestingDatabaseContext();

        var mrBusy = db.Teachers.Where(p => p.Name == "Mr. Busy").First();

        Assert.That(mrBusy.Courses, Has.Count.EqualTo(2));
        Assert.That(mrBusy.Courses.ConvertAll(s => s.Name), Is.EqualTo(["English", "History"]));
    }

    [Test]
    public async Task Get_All_MrBusy_Students()
    {
        var db = TestingContext.GetTestingDatabaseContext();

        var mrBusy = db.Teachers.Where(p => p.Name == "Mr. Busy").First();
        var mrBusysStudentNames = mrBusy.Courses.SelectMany(s => s.Students).Distinct().Select(s => s.Name).ToList();

        Assert.That(mrBusysStudentNames, Has.Count.EqualTo(3));
        Assert.That(mrBusysStudentNames, Is.EqualTo(["Adam", "Bimmy", "Jimmy"]));
    }
}
