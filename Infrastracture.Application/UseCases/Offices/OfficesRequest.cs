using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Offices;

public record OfficesRequest(int? RegionId, int? FillialId, Guid UserId): IRequest<Response>;