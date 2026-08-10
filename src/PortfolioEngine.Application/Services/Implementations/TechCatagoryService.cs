using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Entities;
using System.Linq.Expressions;

namespace PortfolioEngine.Application.Services.Implementations;

public class TechCategoryService : ITechCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TechCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TechCategoryDto>> GetAllCategoriesAsync(Expression<Func<TechCategory, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        var categoryRepo = _unitOfWork.Repository<TechCategory, Guid>();


        var categories = await categoryRepo.GetAllAsync(includes ?? null, cancellationToken);

        return _mapper.Map<IEnumerable<TechCategoryDto>>(categories);
    }

    public async Task<TechCategoryDto?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var categoryRepo = _unitOfWork.Repository<TechCategory, Guid>();
        var category = await categoryRepo.GetByIdAsync(id, cancellationToken);

        return category == null ? null : _mapper.Map<TechCategoryDto>(category);
    }

    public async Task<Guid> CreateCategoryAsync(CreateTechCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var categoryRepo = _unitOfWork.Repository<TechCategory, Guid>();

        var entity = _mapper.Map<TechCategory>(dto);

        await categoryRepo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var categoryRepo = _unitOfWork.Repository<TechCategory, Guid>();
        var category = await categoryRepo.GetByIdAsync(id, cancellationToken);

        if (category != null)
        {
            categoryRepo.Delete(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateCategoryAsync(Guid Id, CreateTechCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var categoryRepo = _unitOfWork.Repository<TechCategory, Guid>();
        var category = await categoryRepo.GetByIdAsync(Id, cancellationToken);

        if (category != null)
        {
            _mapper.Map(dto, category);
            categoryRepo.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync()
    {
        var categoriesRepo = _unitOfWork.Repository<TechCategory, Guid>();
        var categories = await categoriesRepo.GetAllAsync();
        var selectList = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        });
        return selectList;
    }
}