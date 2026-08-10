using Microsoft.AspNetCore.Mvc.Rendering;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Application.Services.Interfaces;

public interface ITechCategoryService
{
    Task<IEnumerable<TechCategoryDto>> GetAllCategoriesAsync(Expression<Func<TechCategory, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<TechCategoryDto?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateCategoryAsync(CreateTechCategoryDto dto, CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateCategoryAsync(Guid Id, CreateTechCategoryDto vm, CancellationToken cancellationToken = default);
    Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync();
}
