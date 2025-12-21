using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.AdminAppointment;

public record AdminAppointmentRequest(Guid UserId, int RegionId): IRequest<Response>;