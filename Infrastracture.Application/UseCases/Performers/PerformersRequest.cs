using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Performers;

public record PerformersRequest(int Page, List<string?> Categories, List<Guid?> OfficeIds, string? Fullname, Guid? UserId): IRequest<Response>;