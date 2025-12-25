using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.Category.Create;

public class CategoryCreateHandler: IRequestHandler<CategoryCreateRequest, Response>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryCreateHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Response> Handle(CategoryCreateRequest request, CancellationToken cancellationToken)
    {
        _categoryRepository.AddCategory(request.Name, request.Description, request.Sla);
        await _unitOfWork.Commit(cancellationToken);
        return new Response("Category created", 200);
    }
}