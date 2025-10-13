using Expenses.App.API.Entities;
using Expenses.App.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.App.API.Database.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(500);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Description)
               .IsRequired()
               .HasMaxLength(500);

        // foreign key

        builder.Property(c => c.UserId).HasMaxLength(500);

        // relations map

        builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Tags)
               .WithMany()
               .UsingEntity<CategoryTag>();

        //builder.HasMany(c => c.Products)
        //       .WithOne(p => p.Category)
        //       .HasForeignKey(p => p.CategoryId);
    }
}
