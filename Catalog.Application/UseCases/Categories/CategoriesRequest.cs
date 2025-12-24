using Catalog.Application.HandlerResponse;
using MediatR;

namespace Catalog.Application.UseCases.Categories;

public record CategoriesRequest(
    int page) : IRequest<Response>;