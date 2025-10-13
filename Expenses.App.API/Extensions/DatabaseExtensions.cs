using Expenses.App.API.Database;
using Expenses.App.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Extensions;

public static class DatabaseExtensions
{
    // update database on development mode ----------------------------------

    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        await using ApplicationDbContext applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await using ApplicationIdentityDbContext identityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

        try
        {
            await applicationDbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Application Database system migration applied successfullly");

            await identityDbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Identiity Database migration applied successfullly");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "error while apply database migrations");
        }
    }

    // add roles to user --------------------------------------------------------

    public static async Task SeedInitialDataAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            if (!await roleManager.RoleExistsAsync(Roles.Member))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Member));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            app.Logger.LogInformation("Succesfully create roles");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occured while seeding initial data");
            throw;
        }
    }
}
