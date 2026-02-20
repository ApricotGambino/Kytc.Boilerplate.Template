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
