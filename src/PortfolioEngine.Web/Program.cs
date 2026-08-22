using PortfolioEngine.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();

// HTTP Pipeline Execution Order
app.UseOpenApiAndScalar();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// CORS -> RateLimiter -> Auth Pipeline
app.UseCors(PortfolioEngine.Web.Extensions.ServiceCollectionExtensions.CorsPolicyName);
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// Route Mappings
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapControllers();
// since im using the rate limiting on "BaseApiController" no need to make global
//app.MapControllers()
// .RequireRateLimiting(PortfolioEngine.Web.Extensions.ServiceCollectionExtensions.RateLimitingPolicyName);



app.MapFallbackToFile("index.html");

// Startup Seed
await app.SeedDatabaseAsync();

app.Run();