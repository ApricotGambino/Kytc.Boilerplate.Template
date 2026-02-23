
using TestShared.Fixtures;
using TestShared.TestObjects;

namespace Sandbox.Concept_Demos.HowDoIAddData;
/// <summary>
/// These tests just show you different ways you can add data to the database.
/// </summary>
[Category(TestingCategoryConstants.SandboxTests)]
public class HowDoIAddData : SharedContextTestFixture
{
    [Test]
    public async Task Add_Single_Entity()
    {
        //When you're adding a single entity, it's pretty straight forward:
        var db = TestingContext.GetTestingDatabaseContext();
        var studentName = $"student from {nameof(Add_Single_Entity)}";
        var studentToAdd = new Student() { Name = studentName };

        db.Students.Add(studentToAdd);
        await db.SaveChangesAsync();

        var foundStudent = db.Students.Where(p => p.Name == studentName).Single();

        Assert.That(studentToAdd.Name, Is.EqualTo(foundStudent.Name));
    }

    [Test]
    public async Task Adding_One_To_Many_Entities_The_Intentional_Way()
    {
        //If we wanted to add an entity, that relies on another entity, we can do this
        //a few ways, this is the most methodical, way, where you're only relying on inserting
        //values by IDs.

        //This way works, but it's a a bit tedious, but you know exactly what's going on at all times.
        var db = TestingContext.GetTestingDatabaseContext();
        var teacherName = $"Teacher from {nameof(Adding_One_To_Many_Entities_The_Intentional_Way)}";
        var teacherToAdd = new Teacher() { Name = teacherName };
        var courseName = $"Course from {nameof(Adding_One_To_Many_Entities_The_Intentional_Way)}";
        var courseToAdd = new Course() { Name = courseName };

        db.Teachers.Add(teacherToAdd);
        await db.SaveChangesAsync();

        courseToAdd.TeacherId = teacherToAdd.Id; //The entity object's ID is updated after the DB save

        db.Courses.Add(courseToAdd);
        await db.SaveChangesAsync();

        var foundTeacher = db.Teachers.Where(p => p.Name == teacherName).Single();
        var foundCourse = db.Courses.Where(p => p.Name == courseName).Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(courseToAdd.Name, Is.EqualTo(foundCourse.Name));
            Assert.That(teacherToAdd.Name, Is.EqualTo(foundTeacher.Name));
            Assert.That(courseToAdd.TeacherId, Is.EqualTo(teacherToAdd.Id));
        }
    }

    [Test]
    public async Task Adding_One_To_Many_Entities_The_Easy_But_Magic_Way()
    {
        //This way feels a bit magic.
        //You'll notice we only ever add a course to the DB, then save the DB, and instead
        //of setting the TeacherId on the course object, we just directly set the object
        //of the teacher that we haven't even added to the database yet!

        //By creating the teacher object, and attaching it to the navigation property of
        //the course, it all gets wrapped up and saved off to the DB.

        //This is an easy way to write your code, but as you can see, it's going to do things
        //that you might not immediately expect. So if you're into doing adding this way,
        //just be cautious to avoid adding things you really didn't expect.
        var db = TestingContext.GetTestingDatabaseContext();
        var teacherName = $"Teacher from {nameof(Adding_One_To_Many_Entities_The_Easy_But_Magic_Way)}";
        var teacherToAdd = new Teacher() { Name = teacherName };
        var courseName = $"Course from {nameof(Adding_One_To_Many_Entities_The_Easy_But_Magic_Way)}";
        var courseToAdd = new Course() { Name = courseName, Teacher = teacherToAdd };

        db.Courses.Add(courseToAdd);
        await db.SaveChangesAsync();

        var foundTeacher = db.Teachers.Where(p => p.Name == teacherName).Single();
        var foundCourse = db.Courses.Where(p => p.Name == courseName).Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(teacherToAdd.Name, Is.EqualTo(foundTeacher.Name));
            Assert.That(courseToAdd.Name, Is.EqualTo(foundCourse.Name));
            Assert.That(courseToAdd.TeacherId, Is.EqualTo(teacherToAdd.Id));
        }
    }

    [Test]
    public async Task Adding_Many_To_Many_Entities_The_Intentional_Way()
    {
        //Adding entities like this is perfectly fine, but it is quite lengthy.
        var db = TestingContext.GetTestingDatabaseContext();
        var teacherName = $"Teacher from {nameof(Adding_Many_To_Many_Entities_The_Intentional_Way)}";
        var teacherToAdd = new Teacher() { Name = teacherName };
        var studentName = $"student from {nameof(Adding_Many_To_Many_Entities_The_Intentional_Way)}";
        var studentToAdd = new Student() { Name = studentName };
        var courseName = $"Course from {nameof(Adding_Many_To_Many_Entities_The_Intentional_Way)}";
        var courseToAdd = new Course() { Name = courseName };
        var studentToCourseToAdd = new StudentToCourse();

        db.Students.Add(studentToAdd);
        await db.SaveChangesAsync();

        db.Teachers.Add(teacherToAdd);
        await db.SaveChangesAsync();

        courseToAdd.TeacherId = teacherToAdd.Id; //The entity object's ID is updated after the DB save

        db.Courses.Add(courseToAdd);
        await db.SaveChangesAsync();

        studentToCourseToAdd.CourseId = courseToAdd.Id;
        studentToCourseToAdd.StudentId = studentToAdd.Id;

        db.StudentToCourses.Add(studentToCourseToAdd);
        await db.SaveChangesAsync();

        var foundStudent = db.Students.Where(p => p.Name == studentName).Single();
        var foundCourse = db.Courses.Where(p => p.Name == courseName).Single();
        var foundStudentToCourse = db.StudentToCourses.Where(p => p.StudentId == studentToAdd.Id && p.CourseId == courseToAdd.Id).Single();
        var foundTeacher = db.Teachers.Where(p => p.Name == teacherName).Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(studentToAdd.Name, Is.EqualTo(foundStudent.Name));
            Assert.That(courseToAdd.Name, Is.EqualTo(foundCourse.Name));
            Assert.That(foundStudentToCourse.CourseId, Is.EqualTo(foundCourse.Id));
            Assert.That(foundStudentToCourse.StudentId, Is.EqualTo(foundStudent.Id));
            Assert.That(teacherToAdd.Name, Is.EqualTo(foundTeacher.Name));
            Assert.That(courseToAdd.TeacherId, Is.EqualTo(teacherToAdd.Id));
        }
    }

    [Test]
    public async Task Adding_Many_To_Many_Entities_The_Easy_But_Magic_Way()
    {
        //This is much cleaner, but also requires you really understand
        //how the entities are composed.  There's no reason you can't do this, just always make sure you're doing what
        //you meant to do.
        var db = TestingContext.GetTestingDatabaseContext();
        var teacherName = $"Teacher from {nameof(Adding_Many_To_Many_Entities_The_Easy_But_Magic_Way)}";
        var studentName = $"student from {nameof(Adding_Many_To_Many_Entities_The_Easy_But_Magic_Way)}";
        var courseName = $"Course from {nameof(Adding_Many_To_Many_Entities_The_Easy_But_Magic_Way)}";


        var teacherToAdd = new Teacher() { Name = teacherName };
        var studentToAdd = new Student() { Name = studentName };
        var courseToAdd = new Course() { Name = courseName, Teacher = teacherToAdd };

        var studentToCourseToAdd = new StudentToCourse()
        {
            Student = studentToAdd,
            Course = courseToAdd,
        };

        db.StudentToCourses.Add(studentToCourseToAdd);
        await db.SaveChangesAsync();

        var foundStudent = db.Students.Where(p => p.Name == studentName).Single();
        var foundCourse = db.Courses.Where(p => p.Name == courseName).Single();
        var foundStudentToCourse = db.StudentToCourses.Where(p => p.StudentId == studentToAdd.Id && p.CourseId == courseToAdd.Id).Single();
        var foundTeacher = db.Teachers.Where(p => p.Name == teacherName).Single();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(studentToAdd.Name, Is.EqualTo(foundStudent.Name));
            Assert.That(courseToAdd.Name, Is.EqualTo(foundCourse.Name));
            Assert.That(foundStudentToCourse.CourseId, Is.EqualTo(foundCourse.Id));
            Assert.That(foundStudentToCourse.StudentId, Is.EqualTo(foundStudent.Id));
            Assert.That(teacherToAdd.Name, Is.EqualTo(foundTeacher.Name));
            Assert.That(courseToAdd.TeacherId, Is.EqualTo(teacherToAdd.Id));
        }
    }
}
