using Expenses.App.API.Entities;
using Expenses.App.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Expenses.App.API.Database.Configurations;

public sealed class CategoryTagConfiguration : IEntityTypeConfiguration<CategoryTag>
{
    public void Configure(EntityTypeBuilder<CategoryTag> builder)
    {
        builder.HasKey(ct => new { ct.CategoryId, ct.TagId });

        builder.HasOne<Tag>()
               .WithMany()
               .HasForeignKey(ct => ct.TagId);

        builder.HasOne<Category>()
               .WithMany(c => c.CategoryTags)
               .HasForeignKey(ct => ct.CategoryId);
    }
}
