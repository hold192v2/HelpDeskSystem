using Catalog.Domain.Interfaces;
using MassTransit;
using TicketService.Application.DTOs;

namespace Catalog.Application.RabbitMq;

public class TicketInfoCatalogConsumer : IConsumer<TicketInfoCatalogRequestDto>
{
    private readonly ICategoryRepository  _categoryRepository;
    private readonly IPriorityRepository _priorityRepository;
    private readonly IStatusRepository _statusRepository;

    public TicketInfoCatalogConsumer(ICategoryRepository categoryRepository, IPriorityRepository priorityRepository,
        IStatusRepository statusRepository)
    {
        _categoryRepository = categoryRepository;
        _priorityRepository = priorityRepository;
        _statusRepository = statusRepository;
    }
    public async Task Consume(ConsumeContext<TicketInfoCatalogRequestDto> context)
    {
        var category = await _categoryRepository.GetCategoryNameById(context.Message.CategoryId);
        var priority = await _priorityRepository.GetPriorityNameById(context.Message.PriorityId);
        var status = await _statusRepository.GetStatusNameById(context.Message.StatusId);
        await context.RespondAsync(new TicketInfoCatalogDto(status, priority, category));
    }
}