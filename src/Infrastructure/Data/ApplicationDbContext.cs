

//using Domain.Entities.Admin;
//using Domain.Entities.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Data
//{
//    //public class ApplicationDbContext : DbContext
//    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
//    //public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
//    //public class ApplicationDbContext() : DbContext()
//    //public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
//        public DbSet<Log> Logs => this.Set<Log>();
//        public DbSet<Test> Tests => this.Set<Test>();
//        //public ApplicationDbContext() : base() { }
//        //This Works: 

//        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        //{
//        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template;Trusted_Connection=True;MultipleActiveResultSets=true");
//        //}

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            //This looks for classes in this assembly that implement the EntityTypeBuilder<TEntity> interface
//            //and applies those specific rules. These are meant to be stored in the EntityConfigurations folder.
//            //TODO: Add one of these. 
//            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
//        }

//        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
//        {
//            //await PreSaveEvents();
//            return await base.SaveChangesAsync(cancellationToken);
//        }

//        //Here we're making SaveChanges() call SaveChangesAsync() that way all our modificates are ensured to be hit if someone forgets to use SaveChangesAsync()
//        public override int SaveChanges() => this.SaveChangesAsync().GetAwaiter().GetResult();


//        //public async Task PreSaveEvents(bool hardDelete = false)
//        //{
//        //    //await ChangeTracker.NotifyUserOfChange(_userNotificationService);
//        //    //await ChangeTracker.SetAuditProperties(_currentAuthorizedUserService, hardDelete);
//        //}

//    }
//}


using System.Reflection;
using Domain.Entities.Admin;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//public class ApplicationDbContext : IdentityDbContext<ApplicationUser>//, IApplicationDbContext
//{
//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//    public DbSet<Log> Logs => Set<Log>();

//    protected override void OnModelCreating(ModelBuilder builder)
//    {
//        base.OnModelCreating(builder);
//        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
//    }
//}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Log> Logs => Set<Log>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

public interface IApplicationDbContext
{
    DbSet<Log> Logs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}


