using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioEngine.Domain.Common;
using PortfolioEngine.Domain.Entities;
using PortfolioEngine.Infrastructure.Persistence.Extensions;
using PortfolioEngine.Infrastructure.Services.System;
using System.Linq.Expressions;

namespace PortfolioEngine.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet for TechCategory entities.
    /// </summary>
    public DbSet<TechCategory> TechCategories { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for Technology entities.
    /// </summary>
    public DbSet<Technology> Technologies { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for Contact entities.
    /// </summary>
    public DbSet<Contact> Contacts { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for Skill entities.
    /// </summary>
    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One-line extension method call
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}