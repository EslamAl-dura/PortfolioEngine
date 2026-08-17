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
    public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync(Expression<Func<Skill, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();
        var skills = await skillRepo.GetAllAsync(includes, cancellationToken);
        return _mapper.Map<IEnumerable<SkillDto>>(skills);
    }

    public async Task<SkillDto?> GetSkillByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();
        var skill = await skillRepo.GetByIdAsync(id, cancellationToken);
        return skill == null ? null : _mapper.Map<SkillDto>(skill);
    }

    public async Task<Guid> CreateSkillAsync(CreateSkillDto dto, CancellationToken cancellationToken = default)
    {
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();
        var techRepo = _unitOfWork.Repository<Technology, Guid>();

        var entity = _mapper.Map<Skill>(dto);

        var distinctTechIds = dto.TechnologyIds?.Distinct().ToList() ?? new List<Guid>();
        if (distinctTechIds.Count != 0)
        {
            var techs = await techRepo.FindAsync(
                    t => distinctTechIds.Contains(t.Id),
                    true,
                    cancellationToken
                );

            foreach (var tech in techs)
            {
                entity.Technologies.Add(tech);
            }
        }

        await skillRepo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateSkillAsync(Guid id, UpdateSkillDto dto, CancellationToken cancellationToken = default)
    {
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();
        var techRepo = _unitOfWork.Repository<Technology, Guid>();

        var entity = await skillRepo.GetByIdWithIncludesAsync(id, [s => s.Technologies], cancellationToken);

        if (entity is null) return;

        _mapper.Map(dto, entity);

        var distinctTechIds = dto.TechnologyIds?.Distinct().ToList() ?? new List<Guid>();

        // Remove technologies that are no longer wanted
        var toRemove = entity.Technologies
            .Where(t => !distinctTechIds.Contains(t.Id))
            .ToList();

        foreach (var tech in toRemove)
            entity.Technologies.Remove(tech);

        var currentIds = entity.Technologies.Select(t => t.Id).ToHashSet();
        var toAddIds = distinctTechIds.Except(currentIds).ToList();

        if (toAddIds.Count > 0)
        {
            var techsToAdd = await techRepo.FindAsync(
                t => toAddIds.Contains(t.Id),
                track: true,               // important!
                cancellationToken);

            foreach (var tech in techsToAdd)
                entity.Technologies.Add(tech);
        }
        //skillRepo.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSkillAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Skill, Guid>();
        var entity = await repo.GetByIdAsync(id);

        if (entity != null)
        {
            repo.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}

