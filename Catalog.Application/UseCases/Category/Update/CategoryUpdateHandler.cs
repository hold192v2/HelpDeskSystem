using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.Category.Update;

public class CategoryUpdateHandler: IRequestHandler<CategoryUpdateRequest, Response>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUpdateHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Response> Handle(CategoryUpdateRequest request, CancellationToken cancellationToken)
    {
        _categoryRepository.UpdateCategory(request.Id, request.Name, request.Sla);
        return new Response("Category updated", 200);
    }
}