using PortfolioEngine.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.Services.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync(CancellationToken cancellationToken = default);
    }
}
