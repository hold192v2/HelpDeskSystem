using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Performers;

public class PerformersGetCartsRequest() : IRequest<Response>
{
    public int Page { get; set; }
    public List<Guid?> Categories { get; set; } = new();
    public List<Guid?> OfficeIds { get; set; } = new();
    public string? Fullname { get; set; } = "";
    public Guid? UserId { get; set; } = null!;


}