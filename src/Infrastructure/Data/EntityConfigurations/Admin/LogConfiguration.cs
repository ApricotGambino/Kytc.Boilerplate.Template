namespace Infrastructure.Data.EntityConfigurations.Admin;
using Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder) => builder.Property(t => t.Properties)
            .HasColumnType("Xml");
}
