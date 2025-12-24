using Catalog.Application.HandlerResponse;
using MediatR;

namespace Catalog.Application.UseCases.PriorityUpdate;

public record PriorityUpdateRequest(
    int Id,
    string Name,
    double Sla): IRequest<Response>;