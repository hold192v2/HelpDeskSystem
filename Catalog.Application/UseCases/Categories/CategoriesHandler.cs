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
        var totalPages = (int)Math.Round((double)totalRecords / 20, MidpointRounding.ToPositiveInfinity);
        var pagination = new PaginationDto(pageIndex, totalRecords, totalPages);

        var contents = categories
            .Select(category => 
            new ContentDto(category.Id, category.Name, category.Description, category.BasePeriodSla)
            ).ToList();
        
        var content = new List<ContentDto>();
        if ((pageIndex - 1) * 20 + 20 > totalRecords)
            content = contents.Slice((pageIndex - 1) * 20, totalRecords - (pageIndex - 1) * 20);
        else
            content = contents.Slice((pageIndex - 1) * 20, 20);
        
        return new Response("Categories", 200, new CategoriesDto(content, pagination));
    }
}