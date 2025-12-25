using Catalog.Application.HandlerResponse;
using MediatR;

namespace Catalog.Application.UseCases.Category.Update;

public record CategoryUpdateRequest(
    Guid Id,
    string Name,
    string Description,
    int Sla): IRequest<Response>;