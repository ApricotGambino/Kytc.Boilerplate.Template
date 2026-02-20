namespace TestShared.TestObjects;

using KernelData.Entities;





/// <summary>
/// Parent of StudentToCourse
/// </summary>
public class Student : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property, can be loaded with .include()
    /// </summary>
    public List<Course>? Courses { get; set; }
}

/// <summary>
/// Depends on Teacher - Course is a child of Teacher
/// Parent of StudentToCourse
/// </summary>
public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int TeacherId { get; set; }

    /// <summary>
    /// Navigation property, can be loaded with .include()
    /// </summary>
    public Teacher? Teacher { get; set; }

    /// <summary>
    /// Navigation property, can be loaded with .include()
    /// </summary>
    public List<Student>? Students { get; set; }
}

/// <summary>
/// Depends on Course - StudentToCourse is a child of Course
/// Depends on Student - StudentToCourse is a child of Student
/// </summary>
public class StudentToCourse : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }

    /// <summary>
    /// Navigation property, can be loaded with .include()
    /// </summary>
    public Student? Student { get; set; }

    /// <summary>
    /// Navigation property, can be loaded with .include()
    /// </summary>
    public Course? Course { get; set; }
}

/// <summary>
/// Parent of Course, but a teacher may not teach any courses.
/// </summary>
public class Teacher : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to courses
    /// </summary>
    public List<Course>? Courses { get; set; }
}


public static class SchoolEntityHelper
{
    /// <summary>
    /// This will insert data into the database as described by the scenario found in the documentation ef-relationships.md
    /// </summary>
    /// <returns></returns>
    public static Task InsertExampleSchoolSetup()
    {
        var db = TestingContext.GetTestingDatabaseContext();
        var adam = new Student() { Name = "Adam" };
        var jimmy = new Student() { Name = "Jimmy" };
        var bimmy = new Student() { Name = "Bimmy" };
        db.Add(adam);
        db.Add(jimmy);
        db.Add(bimmy);

        var mrBusy = new Teacher() { Name = "Mr. Busy" };
        var mrPlop = new Teacher() { Name = "Mr. Plop" };
        var drLazy = new Teacher() { Name = "Dr. Lazy" };
        db.Add(mrBusy);
        db.Add(mrPlop);
        db.Add(drLazy);

        var english = new Course() { Name = "English", Teacher = mrBusy };
        var history = new Course() { Name = "History", Teacher = mrBusy };
        var math = new Course() { Name = "Math", Teacher = mrPlop };
        db.Add(english);
        db.Add(history);
        db.Add(math);

        db.Add(new StudentToCourse() { Student = adam, Course = english });

        db.Add(new StudentToCourse() { Student = jimmy, Course = history });
        db.Add(new StudentToCourse() { Student = jimmy, Course = math });

        db.Add(new StudentToCourse() { Student = bimmy, Course = english });
        db.Add(new StudentToCourse() { Student = bimmy, Course = history });
        db.Add(new StudentToCourse() { Student = bimmy, Course = math });

        return db.SaveChangesAsync();
    }
}
