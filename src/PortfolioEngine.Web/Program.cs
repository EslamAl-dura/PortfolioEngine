using Microsoft.AspNetCore.Identity;
using PortfolioEngine.Application;
using PortfolioEngine.Infrastructure;
using PortfolioEngine.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PortfolioEngine.Domain.Entities;
using AutoMapper;
using PortfolioEngine.Application.Common.Mappings;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


// ==========================================
// 1. identify the Database Provider and Configure EF Core
// ==========================================
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// 2. Configure Cookie Paths
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// ==========================================
// 2. Service Registrations (Modular DI)
// ==========================================

// Add AutoMapper with Mapping Profiles
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(TechMappingProfile).Assembly);
});

// Add MVC Controllers & Views for Admin Dashboard
builder.Services.AddControllersWithViews();

// Add OpenAPI Metadata Support for Scalar
builder.Services.AddOpenApi();


// Add Core Application & Infrastructure Layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// ==========================================
// 3. HTTP Request Pipeline Configuration
// ==========================================

if (app.Environment.IsDevelopment())
{
    // Generate OpenAPI Spec Endpoint
    app.MapOpenApi();

    // Configure Modern Scalar UI for API Testing
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Portfolio Engine API Reference")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch);
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// Route configuration: Admin MVC UI & Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Map Razor Pages for Identity UI
app.MapRazorPages();

// Map REST API Controllers
app.MapControllers();


// Seed initial database data during startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await ApplicationDbContextSeed.RunAutoMigration(context);
        await ApplicationDbContextSeed.SeedAdminUserAsync(services.GetRequiredService<UserManager<ApplicationUser>>());
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// SPA Fallback for Angular/React/Vue apps
app.MapFallbackToFile("index.html");

app.Run();