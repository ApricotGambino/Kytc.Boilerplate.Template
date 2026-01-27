namespace Infrastructure.Data;

using System.Reflection;
using Domain.Entities.Admin;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<Log> Logs { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}