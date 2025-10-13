using Expenses.App.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.App.API.Database.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasMaxLength(500);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(t => t.Description).HasMaxLength(500);

        builder.HasIndex(t => new { t.UserId, t.Name }).IsUnique();

        // relations map

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(t => t.UserId);
    }
}
