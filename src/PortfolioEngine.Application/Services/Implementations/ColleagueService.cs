using AutoMapper;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PortfolioEngine.Application.Services.Implementations;

public class ColleagueService : IColleagueService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ColleagueService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ColleagueDto>> GetAllColleaguesAsync(
        Expression<Func<Colleague, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        var colleagueRepo = _unitOfWork.Repository<Colleague, Guid>();

        if (includes is null or { Length: 0 })
        {
            return await colleagueRepo.GetAllProjectedAsync(
                selector: c => new ColleagueDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Role = c.Role,
                    AvatarUrl = c.AvatarUrl,
                    ProfileUrl = c.ProfileUrl,
                    UntilNow = c.UntilNow,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate
                },
                cancellationToken: cancellationToken);
        }

        var colleagues = await colleagueRepo.GetAllAsync(
            includes: includes,
            track: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyList<ColleagueDto>>(colleagues);
    }

    public async Task<ColleagueDto?> GetColleagueByIdAsync(
        Guid id,
        Expression<Func<Colleague, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        var colleagueRepo = _unitOfWork.Repository<Colleague, Guid>();

        if (includes is null or { Length: 0 })
        {
            return await colleagueRepo.GetByIdProjectedAsync(
                id,
                selector: c => new ColleagueDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Role = c.Role,
                    AvatarUrl = c.AvatarUrl,
                    ProfileUrl = c.ProfileUrl,
                    UntilNow = c.UntilNow,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate
                },
                cancellationToken: cancellationToken);
        }

        var colleague = await colleagueRepo.GetByIdWithIncludesAsync(
            id,
            includes: includes,
            track: false,
            cancellationToken: cancellationToken);

        return colleague is null ? null : _mapper.Map<ColleagueDto>(colleague);
    }

    public async Task<Guid> CreateColleagueAsync(
        CreateColleagueDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var colleagueRepo = _unitOfWork.Repository<Colleague, Guid>();
        var entity = _mapper.Map<Colleague>(dto);

        await colleagueRepo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateColleagueAsync(
        Guid id,
        UpdateColleagueDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var colleagueRepo = _unitOfWork.Repository<Colleague, Guid>();
        var entity = await colleagueRepo.GetByIdAsync(
            id,
            track: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            return;

        _mapper.Map(dto, entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteColleagueAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var colleagueRepo = _unitOfWork.Repository<Colleague, Guid>();
        var entity = await colleagueRepo.GetByIdAsync(
            id,
            track: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            return;

        colleagueRepo.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
