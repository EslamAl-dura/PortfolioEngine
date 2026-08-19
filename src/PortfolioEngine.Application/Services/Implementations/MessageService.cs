using AutoMapper;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Common;
using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Application.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<MessageDto>> GetPagedMessagesAsync(
    MessageFilterDto filter,
    CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Message, Guid>();

        Expression<Func<Message, bool>> predicate = m =>
            (string.IsNullOrWhiteSpace(filter.Search) ||
             m.SenderName.Contains(filter.Search) ||
             m.SenderEmail.Contains(filter.Search) ||
             m.Subject.Contains(filter.Search) ||
             m.Content.Contains(filter.Search)) &&
            (!filter.IsRead.HasValue || m.IsRead == filter.IsRead.Value);

        Expression<Func<Message, object>> orderBySelector = filter.SortBy?.ToLower() switch
        {
            "sendername" => x => x.SenderName,
            "senderemail" => x => x.SenderEmail,
            "subject" => x => x.Subject,
            "isread" => x => x.IsRead,
            _ => x => x.CreatedAtUtc
        };

        // 1. Get paged result entity wrapper from repository
        var pagedMessages = await repo.GetPagedAsync(
            predicate: predicate,
            orderBy: orderBySelector,
            ascending: filter.IsDescending,
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            track: false,
            cancellationToken: cancellationToken);

        // 2. Map ONLY the inner Items collection (PagedResult<Message>.Items -> IReadOnlyList<MessageDto>)
        var mappedItems = _mapper.Map<IReadOnlyList<MessageDto>>(pagedMessages.Items);

        // 3. Return the mapped result DTO
        return new PagedResultDto<MessageDto>
        {
            Items = mappedItems,
            TotalCount = pagedMessages.TotalCount, // Or use count from pagedMessages if available
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }

    public async Task<MessageDto?> GetMessageByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Message, Guid>();
        var entity = await repo.GetByIdAsync(id, track: false, cancellationToken: cancellationToken);
        return entity is null ? null : _mapper.Map<MessageDto>(entity);
    }

    public async Task ToggleReadStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Message, Guid>();
        var entity = await repo.GetByIdAsync(id, track: true, cancellationToken: cancellationToken);

        if (entity is null) return;

        entity.IsRead = !entity.IsRead;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Message, Guid>();
        var entity = await repo.GetByIdAsync(id, track: true, cancellationToken: cancellationToken);

        if (entity is null) return;

        repo.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

}