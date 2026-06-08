using Demo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Infrastructure.Data.Configuration;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value));
        builder.ToTable("Users")
            .HasKey(e=>e.Id);
        builder.HasIndex(e => e.Email)
            .IsUnique();
    }
}
