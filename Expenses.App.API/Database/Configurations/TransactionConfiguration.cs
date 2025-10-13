//using Expenses.App.API.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Expenses.App.API.Database.Configurations;

//public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
//{
//    public void Configure(EntityTypeBuilder<Transaction> builder)
//    {
//        builder.HasKey(t => t.TransactionId);

//        builder.Property(t => t.ProductName)
//            .IsRequired()
//            .HasMaxLength(150);

//        builder.Property(t => t.Price)
//            .IsRequired();

//        builder.HasOne(t => t.User)
//            .WithMany(u => u.Transactions)
//            .HasForeignKey(t => t.UserId);
//    }
//}
