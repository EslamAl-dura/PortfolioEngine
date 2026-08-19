using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.Services.Interfaces;

public interface IMessageService
{
    Task<PagedResultDto<MessageDto>> GetPagedMessagesAsync(
        MessageFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<MessageDto?> GetMessageByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task ToggleReadStatusAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task DeleteMessageAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
