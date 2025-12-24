using Catalog.Application.DTOs;
using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.Categories;

public class CategoriesHandler: IRequestHandler<CategoriesRequest, Response>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Response> Handle(CategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository.GetAllCategories().Result;
        
        var pageIndex = request.page;
        var totalRecords = categories.Count;
        var totalPages = (int)MathF.Ceiling(categories.Count / 20);
        var pagination = new PaginationDto(pageIndex, totalRecords, totalPages);

        var contents = categories
            .Select(category => 
            new ContentDto(category.Id, category.Name, category.Description, category.BasePeriodSla)
            ).ToList();
        
        return new Response("Categories", 200, new CategoriesDto(contents, pagination));
    }
}