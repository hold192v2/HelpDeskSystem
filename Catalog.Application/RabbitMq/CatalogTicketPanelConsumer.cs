using Catalog.Domain.Interfaces;
using MassTransit;
using TicketService.Application.DTOs;

namespace Catalog.Application.RabbitMq;

public class CatalogTicketPanelConsumer : IConsumer<CatalogTicketPanelRequestDto>
{
    private readonly IPriorityRepository  _priorityRepository;
    private readonly IStatusRepository _statusRepository;
    public CatalogTicketPanelConsumer(IPriorityRepository priorityRepository, IStatusRepository statusRepository)
    {
        _priorityRepository = priorityRepository;
        _statusRepository = statusRepository;
    }
    public async Task Consume(ConsumeContext<CatalogTicketPanelRequestDto> context)
    {
        var priorityNames =  await _priorityRepository.GetPriorityNamesByIds(context.Message.PriorityIds);
        var statusNames = await _statusRepository.GetStatusNamesByIds(context.Message.StatusIds);
        await context.RespondAsync(new CatalogTicketPanelDto(statusNames, priorityNames));
    }
}