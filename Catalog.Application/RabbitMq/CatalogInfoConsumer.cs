using Catalog.Application.DTOs;
using Catalog.Domain.Interfaces;
using MassTransit;

namespace Catalog.Application.RabbitMq;

public class CatalogInfoConsumer : IConsumer<CatalogInfoRequestDto>
{
    private readonly ICategoryRepository  _categoryRepository;

    public CatalogInfoConsumer(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task Consume(ConsumeContext<CatalogInfoRequestDto> context)
    {
        var categoryIds = context.Message.CategoryIds;
        var categoryNames = await _categoryRepository.GetCategoryNames(categoryIds);
        await context.RespondAsync(new CatalogInfoNameDto(categoryNames));
    }
}