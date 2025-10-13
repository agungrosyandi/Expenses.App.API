//using Expenses.App.API.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Expenses.App.API.Database.Configurations;

//public class ProductConfiguration : IEntityTypeConfiguration<Product>
//{
//    public void Configure(EntityTypeBuilder<Product> builder)
//    {
//        builder.HasKey(p => p.Id);
//        builder.Property(p => p.Id).HasMaxLength(500);

//        builder.Property(p => p.Name)
//               .IsRequired()
//               .HasMaxLength(100);

//        builder.Property(p => p.Quantity)
//               .IsRequired();

//        builder.Property(p => p.Price)
//               .IsRequired();

//        // relations map

//        builder.HasOne<Category>()
//               .WithMany(c => c.Products)
//               .HasForeignKey(p => p.CategoryId);
//    }
//}
