using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortfolioEngine.Domain.Entities;

namespace PortfolioEngine.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{   
    public static async Task RunAutoMigration(ApplicationDbContext context)
    {
        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        if (!await userManager.Users.AnyAsync())
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@portfolioengine.local",
                Email = "admin@portfolioengine.local",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, "Admin@123456");
        }
    }
}