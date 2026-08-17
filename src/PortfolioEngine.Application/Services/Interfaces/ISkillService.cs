using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Entities;
using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Application.Services.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync(Expression<Func<Skill, object>>[]? includes = null, CancellationToken cancellationToken = default);
        Task<SkillDto?> GetSkillByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> CreateSkillAsync(CreateSkillDto dto, CancellationToken cancellationToken = default);
        Task UpdateSkillAsync(Guid id, UpdateSkillDto dto, CancellationToken cancellationToken = default);
        Task DeleteSkillAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
