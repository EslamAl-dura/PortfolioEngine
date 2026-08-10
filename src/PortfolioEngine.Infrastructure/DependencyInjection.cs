using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Infrastructure.Persistence;
using PortfolioEngine.Infrastructure.Persistence.Interceptors;
using PortfolioEngine.Infrastructure.Persistence.Repositories;
using PortfolioEngine.Infrastructure.Services.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register HttpContextAccessor
        services.AddHttpContextAccessor();

        // Register AuditAndSoftDeleteInterceptor
        services.AddScoped<AuditAndSoftDeleteInterceptor>();

        // Register CurrentUserService
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Register ApplicationDbContext with MS SQL Server
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            // Resolve the scoped interceptor from the service provider
            var interceptor = sp.GetRequiredService<AuditAndSoftDeleteInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(interceptor);
        });



        // Register UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register File Storage Service
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        return services;
    }
}
