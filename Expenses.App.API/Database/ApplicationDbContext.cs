using Expenses.App.API.Database.Configurations;
using Expenses.App.API.Database.Schema;
using Expenses.App.API.Entities;
using Expenses.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<CategoryTag> CategoryTags { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schemas.Application);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
