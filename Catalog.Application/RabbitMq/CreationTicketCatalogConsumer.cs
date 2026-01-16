using Catalog.Domain.Interfaces;
using MassTransit;
using TicketService.Application.DTOs;

namespace Catalog.Application.RabbitMq;

public class CreationTicketCatalogConsumer : IConsumer<CreationTicketCatalogRequestDto>
{
    private readonly IPriorityRepository _priorityRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreationTicketCatalogConsumer(ICategoryRepository categoryRepository, IPriorityRepository priorityRepository)
    {
        _priorityRepository = priorityRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task Consume(ConsumeContext<CreationTicketCatalogRequestDto> context)
    {
        var category = await _categoryRepository.GetCategoryById(context.Message.CategoryId);
        var priority = await _priorityRepository.GetPriorityById(context.Message.PriorityId);
        await context.RespondAsync(new CreationTicketCatalogDto(category.BasePeriodSla, priority.SlaFactor));
    }
}