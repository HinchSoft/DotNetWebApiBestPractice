using DemoDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoInfrastructure.Data.Configuration;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value));
        builder.ToTable("Users")
            .HasKey(e => e.Id).HasName("PK_Users");
        builder.HasIndex(e => e.Email)
            .IsUnique();
    }
}
