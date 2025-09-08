using Expenses.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // -------------------------
        // User config
        // -------------------------

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.PublicId).HasDefaultValueSql("NEWID()"); // Auto-generate GUID

            // User → Transactions (1:N)

            entity.HasMany(u => u.Transactions)
                  .WithOne(t => t.User)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // 🔑 cascade delete
        });

        // -------------------------
        // Transaction config
        // -------------------------

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.PublicId).HasDefaultValueSql("NEWID()"); // Auto-generate GUID
        });
    }
}
