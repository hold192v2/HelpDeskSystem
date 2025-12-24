using Catalog.Application.HandlerResponse;
using MediatR;

namespace Catalog.Application.UseCases.Category.Update;

public record CategoryUpdateRequest(
    Guid Id,
    string Name,
    double Sla): IRequest<Response>;