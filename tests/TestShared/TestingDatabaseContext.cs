namespace TestShared;

using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using TestShared.TestObjects;
/// <summary>
/// These entities are used only in the Testing Database Context, this entity should not be added to the ApplicationDBContext.
/// </summary>
/// <param name="options"></param>
public class TestingDatabaseContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
{

    public DbSet<TestEntity> TestEntities { get; set; }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentToCourse> StudentToCourses { get; set; }
    public DbSet<Teacher> Teachers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasMany(e => e.Students)
            .WithMany(e => e.Courses)
            .UsingEntity<StudentToCourse>();
    }
}
