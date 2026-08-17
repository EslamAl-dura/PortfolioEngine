using AutoMapper;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Entities;
using System.Linq.Expressions;

namespace PortfolioEngine.Application.Services.Implementations;

public class SkillService : ISkillService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SkillService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ---------- READ ----------

    public async Task<IReadOnlyList<SkillDto>> GetAllSkillsAsync(
        Expression<Func<Skill, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();

        // Prefer projection when possible (no AutoMapper overhead + less data)
        // Fallback to full entity + map only if complex includes are needed
        if (includes is null or { Length: 0 })
        {
            return await skillRepo.GetAllProjectedAsync(
                selector: s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    // map other simple properties here
                    // TechnologyIds = s.Technologies.Select(t => t.Id).ToList() // if needed
                },
                cancellationToken: cancellationToken);
        }

        // When includes are requested we still materialize entities
        var skills = await skillRepo.GetAllAsync(
            includes: includes,
            track: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyList<SkillDto>>(skills);
    }

    public async Task<SkillDto?> GetSkillByIdAsync(
        Guid id,
        Expression<Func<Skill, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();

        if (includes is null or { Length: 0 })
        {
            return await skillRepo.GetByIdProjectedAsync(
                id,
                selector: s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    // add other properties as needed
                },
                cancellationToken: cancellationToken);
        }

        var skill = await skillRepo.GetByIdWithIncludesAsync(
            id,
            includes: includes,
            track: false,
            cancellationToken: cancellationToken);

        return skill is null ? null : _mapper.Map<SkillDto>(skill);
    }

    // ---------- CREATE ----------

    public async Task<Guid> CreateSkillAsync(
        CreateSkillDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var skillRepo = _unitOfWork.Repository<Skill, Guid>();
        var techRepo = _unitOfWork.Repository<Technology, Guid>();

        var entity = _mapper.Map<Skill>(dto);

        var distinctTechIds = dto.TechnologyIds?
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList() ?? [];

        if (distinctTechIds.Count > 0)
        {
            var techs = await techRepo.FindAsync(
                predicate: t => distinctTechIds.Contains(t.Id),
                track: true,                    // must be tracked to attach to collection
                cancellationToken: cancellationToken);

            // Optional: detect missing IDs
            if (techs.Count != distinctTechIds.Count)
            {
                var foundIds = techs.Select(t => t.Id).ToHashSet();
                var missing = distinctTechIds.Where(id => !foundIds.Contains(id));
                // throw new NotFoundException($"Technologies not found: {string.Join(", ", missing)}");
            }

            foreach (var tech in techs)
            {
                entity.Technologies.Add(tech);
            }
        }

        await skillRepo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    // ---------- UPDATE ----------

    public async Task UpdateSkillAsync(
        Guid id,
        UpdateSkillDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var skillRepo = _unitOfWork.Repository<Skill, Guid>();
        var techRepo = _unitOfWork.Repository<Technology, Guid>();

        // Must be tracked + include the collection we will modify
        var entity = await skillRepo.GetByIdWithIncludesAsync(
            id,
            includes: [s => s.Technologies],
            track: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            return; // or throw NotFoundException

        // Map scalar properties
        _mapper.Map(dto, entity);

        var desiredTechIds = dto.TechnologyIds?
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToHashSet() ?? [];

        // Remove technologies that are no longer desired
        var toRemove = entity.Technologies
            .Where(t => !desiredTechIds.Contains(t.Id))
            .ToList();

        foreach (var tech in toRemove)
            entity.Technologies.Remove(tech);

        // Add missing technologies
        var currentIds = entity.Technologies.Select(t => t.Id).ToHashSet();
        var toAddIds = desiredTechIds.Except(currentIds).ToList();

        if (toAddIds.Count > 0)
        {
            var techsToAdd = await techRepo.FindAsync(
                predicate: t => toAddIds.Contains(t.Id),
                track: true,                    // critical for relationship tracking
                cancellationToken: cancellationToken);

            foreach (var tech in techsToAdd)
                entity.Technologies.Add(tech);
        }

        // No need to call Update() – entity is already tracked
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    // ---------- DELETE ----------

    public async Task DeleteSkillAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();

        // Soft-delete via interceptor → entity must be tracked
        var entity = await skillRepo.GetByIdAsync(
            id,
            track: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            return; // or throw NotFoundException

        skillRepo.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}