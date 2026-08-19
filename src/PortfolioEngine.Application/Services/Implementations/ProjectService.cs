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

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProjectDto>> GetAllProjectsAsync(
        Expression<Func<Project, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        var projectRepo = _unitOfWork.Repository<Project, Guid>();

        var defaultIncludes = new Expression<Func<Project, object>>[]
        {
            p => p.Colleagues,
            p => p.Skills
        };

        var projects = await projectRepo.GetAllAsync(
            includes: includes ?? defaultIncludes,
            track: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyList<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(
        Guid id,
        Expression<Func<Project, object>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        var projectRepo = _unitOfWork.Repository<Project, Guid>();

        var defaultIncludes = new Expression<Func<Project, object>>[]
        {
            p => p.Colleagues,
            p => p.Skills
        };

        var project = await projectRepo.GetByIdWithIncludesAsync(
            id,
            includes: includes ?? defaultIncludes,
            track: false,
            cancellationToken: cancellationToken);

        return project is null ? null : _mapper.Map<ProjectDto>(project);
    }

    public async Task<Guid> CreateProjectAsync(
        CreateProjectDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var projectRepo = _unitOfWork.Repository<Project, Guid>();
        var colleagueRepo = _unitOfWork.Repository<Colleague, Guid>();
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();

        var project = _mapper.Map<Project>(dto);

        // Attach Colleagues
        if (dto.ColleagueIds is { Count: > 0 })
        {
            var colleagues = await colleagueRepo.FindAsync(
                c => dto.ColleagueIds.Contains(c.Id),
                track: true,
                cancellationToken: cancellationToken);

            foreach (var colleague in colleagues)
            {
                project.Colleagues.Add(colleague);
            }
        }

        // Attach Skills
        if (dto.SkillIds is { Count: > 0 })
        {
            var skills = await skillRepo.FindAsync(
                s => dto.SkillIds.Contains(s.Id),
                track: true,
                cancellationToken: cancellationToken);

            foreach (var skill in skills)
            {
                project.Skills.Add(skill);
            }
        }

        await projectRepo.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id;
    }

    public async Task UpdateProjectAsync(
        Guid id,
        UpdateProjectDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var projectRepo = _unitOfWork.Repository<Project, Guid>();
        var colleagueRepo = _unitOfWork.Repository<Colleague, Guid>();
        var skillRepo = _unitOfWork.Repository<Skill, Guid>();

        var includes = new Expression<Func<Project, object>>[]
        {
            p => p.Colleagues,
            p => p.Skills
        };

        var entity = await projectRepo.GetByIdWithIncludesAsync(
            id,
            includes: includes,
            track: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            return;

        // Map primitive properties
        _mapper.Map(dto, entity);

        // Synchronize Colleagues relationship
        entity.Colleagues.Clear();
        if (dto.ColleagueIds is { Count: > 0 })
        {
            var colleagues = await colleagueRepo.FindAsync(
                c => dto.ColleagueIds.Contains(c.Id),
                track: true,
                cancellationToken: cancellationToken);

            foreach (var colleague in colleagues)
            {
                entity.Colleagues.Add(colleague);
            }
        }

        // Synchronize Skills relationship
        entity.Skills.Clear();
        if (dto.SkillIds is { Count: > 0 })
        {
            var skills = await skillRepo.FindAsync(
                s => dto.SkillIds.Contains(s.Id),
                track: true,
                cancellationToken: cancellationToken);

            foreach (var skill in skills)
            {
                entity.Skills.Add(skill);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProjectAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var projectRepo = _unitOfWork.Repository<Project, Guid>();
        var entity = await projectRepo.GetByIdAsync(
            id,
            track: true,
            cancellationToken: cancellationToken);

        if (entity is null)
            return;

        projectRepo.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}