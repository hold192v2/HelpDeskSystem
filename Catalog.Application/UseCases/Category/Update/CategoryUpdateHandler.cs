using Catalog.Application.HandlerResponse;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.UseCases.Category.Update;

public class CategoryUpdateHandler: IRequestHandler<CategoryUpdateRequest, Response>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryUpdateHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Response> Handle(CategoryUpdateRequest request, CancellationToken cancellationToken)
    {
        await _categoryRepository.UpdateCategory(request.CategoryId, request.Name, request.Description, request.Sla);
        await _unitOfWork.Commit(cancellationToken);
        return new Response("Category updated", 200);
    }
}