namespace Sandbox.Concept_Demos.EFRelationship;

using TestShared.Fixtures;
using TestShared.TestObjects;

/// <summary>
/// This is showcasing a working example of the ef-relationships concept in the documentation.
/// </summary>
[Category(TestingCategoryConstants.SandboxTests)]
public class EFRelationship : SharedContextTestFixture
{
    private List<Student> CreatedStudents { get; set; } = [];
    private List<Teacher> CreatedTeachers { get; set; } = [];
    private List<Course> CreatedCourses { get; set; } = [];
    private List<StudentToCourse> CreatedStudentToCourses { get; set; } = [];

    [OneTimeSetUp]
    public Task SeedTestData()
    {
        var db = TestingContext.GetTestingDatabaseContext();
        var adam = new Student() { Name = "Adam" };
        var jimmy = new Student() { Name = "Jimmy" };
        var bimmy = new Student() { Name = "Bimmy" };

        var mrBusy = new Teacher() { Name = "Mr. Busy" };
        var mrPlop = new Teacher() { Name = "Mr. Plop" };
        var drLazy = new Teacher() { Name = "Dr. Lazy" };

        var english = new Course() { Name = "English", Teacher = mrBusy };
        var history = new Course() { Name = "History", Teacher = mrBusy };
        var math = new Course() { Name = "Math", Teacher = mrPlop };

        CreatedStudents.Add(adam);
        CreatedStudents.Add(jimmy);
        CreatedStudents.Add(bimmy);

        CreatedTeachers.Add(mrBusy);
        CreatedTeachers.Add(mrPlop);
        CreatedTeachers.Add(drLazy);

        CreatedCourses.Add(english);
        CreatedCourses.Add(history);
        CreatedCourses.Add(math);

        CreatedStudentToCourses.Add(new StudentToCourse() { Student = adam, Course = english });

        CreatedStudentToCourses.Add(new StudentToCourse() { Student = jimmy, Course = history });
        CreatedStudentToCourses.Add(new StudentToCourse() { Student = jimmy, Course = math });

        CreatedStudentToCourses.Add(new StudentToCourse() { Student = bimmy, Course = english });
        CreatedStudentToCourses.Add(new StudentToCourse() { Student = bimmy, Course = history });
        CreatedStudentToCourses.Add(new StudentToCourse() { Student = bimmy, Course = math });

        db.AddRange(CreatedStudents);
        db.AddRange(CreatedTeachers);
        db.AddRange(CreatedCourses);
        db.AddRange(CreatedStudentToCourses);
        return db.SaveChangesAsync();
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
