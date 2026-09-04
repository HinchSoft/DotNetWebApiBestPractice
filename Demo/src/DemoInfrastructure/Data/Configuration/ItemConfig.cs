using DemoDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoInfrastructure.Data.Configuration;

internal class ItemConfig : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => new ItemId(value));
        builder.ToTable("Items")
            .HasKey(e => e.Id).HasName("PK_Items");
    }
}