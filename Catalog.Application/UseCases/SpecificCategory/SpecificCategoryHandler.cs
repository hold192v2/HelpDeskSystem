using Catalog.Application.DTOs;
using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.SpecificCategory;

public class SpecificCategoryHandler: IRequestHandler<SpecificCategoryRequest, Response>
{
    private readonly ICategoryRepository _categoryRepository;

    public SpecificCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Response> Handle(SpecificCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryById(request.Id);
        return new Response("Specific Category", 200, new SpecificCategoryDto(category.Id, category.Name, category.Description, category.BasePeriodSla));
    }
}