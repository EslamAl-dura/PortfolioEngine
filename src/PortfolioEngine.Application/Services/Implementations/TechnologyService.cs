using AutoMapper;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Application.Services.Implementations;

public class TechnologyService : ITechnologyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TechnologyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TechnologyDto>> GetAllTechnologiesAsync(Expression<Func<Technology, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Technology, Guid>();
        var technologies = await repo.GetAllAsync(includes, cancellationToken);
        return _mapper.Map<IEnumerable<TechnologyDto>>(technologies);
    }

    public async Task<TechnologyDto?> GetTechnologyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Technology, Guid>();
        var tech = await repo.GetByIdAsync(id, cancellationToken);

        return tech == null ? null : _mapper.Map<TechnologyDto>(tech);
    }

    public async Task<Guid> CreateTechnologyAsync(CreateTechnologyDto dto, CancellationToken cancellationToken = default)
    {
        var techRepo = _unitOfWork.Repository<Technology, Guid>();

        var entity = _mapper.Map<Technology>(dto);

        await techRepo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateTechnologyAsync(Guid id, UpdateTechnologyDto dto, CancellationToken cancellationToken = default)
    {
        var techRepo = _unitOfWork.Repository<Technology, Guid>();
        var entity = await techRepo.GetByIdAsync(id, cancellationToken);

        if (entity != null)
        {
            _mapper.Map(dto, entity);
            techRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteTechnologyAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var techRepo = _unitOfWork.Repository<Technology, Guid>();
        var entity = await techRepo.GetByIdAsync(id, cancellationToken);

        if (entity != null)
        {
            techRepo.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}