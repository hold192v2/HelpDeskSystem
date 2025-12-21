using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Performers;

public record PerformersGetCartsRequest(
    int Page,
    List<string?> Categories,
    List<Guid?> OfficeIds,
    string? Fullname = "",
    Guid? UserId = null!) : IRequest<Response>
{
    public List<string?> Categories { get; init; } = new();
    public List<Guid?> OfficeIds { get; init; } = new();

}