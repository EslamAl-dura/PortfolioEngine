using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PortfolioEngine.Domain.Entities;
using PortfolioEngine.Infrastructure.Persistence;
using Scalar.AspNetCore;

namespace PortfolioEngine.Web.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseOpenApiAndScalar(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Portfolio Engine API Reference")
                       .WithTheme(ScalarTheme.Moon)
                       .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch);
                // Enable Bearer token auth in Scalar UI
                options.Authentication = new ScalarAuthenticationOptions
                {
                    // PreferredSecurityScheme is obsolete; use PreferredSecuritySchemes instead
                    PreferredSecuritySchemes = new[] { "Bearer" }
                };
            });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        return app;
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            await ApplicationDbContextSeed.RunAutoMigration(context);

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await ApplicationDbContextSeed.SeedAdminUserAsync(userManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
    }
}