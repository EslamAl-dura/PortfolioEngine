using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PortfolioEngine.Domain.Common;
using PortfolioEngine.Infrastructure.Services.System;

namespace PortfolioEngine.Infrastructure.Persistence.Interceptors;

public class AuditAndSoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public AuditAndSoftDeleteInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var utcNow = DateTime.UtcNow;
        var currentUserId = _currentUserService.UserId;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            // Track initial state before soft-delete transforms Deleted -> Modified
            var originalState = entry.State;

            // --- 1. HANDLE SOFT DELETE ---
            if (originalState == EntityState.Deleted && entry.Entity is ISoftDelete softDeleteEntity)
            {
                entry.State = EntityState.Modified;
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedAtUtc = utcNow;
                softDeleteEntity.DeletedBy = currentUserId;
            }

            // --- 2. HANDLE AUDIT LOGGING ---
            if (entry.Entity is IAuditableEntity auditableEntity)
            {
                if (originalState == EntityState.Added)
                {
                    auditableEntity.CreatedAtUtc = utcNow;
                    auditableEntity.CreatedBy = currentUserId;
                }
                else if (originalState == EntityState.Modified || entry.State == EntityState.Modified)
                {
                    auditableEntity.UpdatedAtUtc = utcNow;
                    auditableEntity.UpdatedBy = currentUserId;

                    // Safely mark creation properties as unmodified during updates
                    entry.Property(nameof(auditableEntity.CreatedAtUtc)).IsModified = false;
                    entry.Property(nameof(auditableEntity.CreatedBy)).IsModified = false;
                }
            }
        }
    }
}