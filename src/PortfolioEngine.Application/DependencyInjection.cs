using Microsoft.Extensions.DependencyInjection;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Application.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register Application Services
        services.AddScoped<ITechCategoryService, TechCategoryService>();
        services.AddScoped<ITechnologyService, TechnologyService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ISkillService, SkillService>();
        return services;
    }
}