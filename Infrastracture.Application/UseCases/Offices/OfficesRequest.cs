using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Offices;

public record OfficesRequest(int? RegioId, int? FillialId): IRequest<Response>;