using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.Category.Create;

public class CategoryCreateHandler: IRequestHandler<CategoryCreateRequest, Response>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryCreateHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Response> Handle(CategoryCreateRequest request, CancellationToken cancellationToken)
    {
        _categoryRepository.AddCategory(request.Name, request.Sla);
        return new Response("Category created", 200);
    }
}