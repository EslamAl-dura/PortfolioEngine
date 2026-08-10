using Microsoft.EntityFrameworkCore;
using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies a global query filter to all entities that implement the ISoftDelete interface, ensuring that only non-deleted entities are returned in queries.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyMethodInfo = typeof(EF).GetMethod(nameof(EF.Property))!
                    .MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(
                    propertyMethodInfo,
                    parameter,
                    Expression.Constant(nameof(ISoftDelete.IsDeleted))
                );
                var compareExpression = Expression.Equal(
                    isDeletedProperty,
                    Expression.Constant(false)
                );
                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}