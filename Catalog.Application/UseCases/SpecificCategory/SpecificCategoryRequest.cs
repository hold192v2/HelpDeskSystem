using Catalog.Application.HandlerResponse;
using MediatR;

namespace Catalog.Application.UseCases.SpecificCategory;

public record SpecificCategoryRequest(
    Guid Id): IRequest<Response>;