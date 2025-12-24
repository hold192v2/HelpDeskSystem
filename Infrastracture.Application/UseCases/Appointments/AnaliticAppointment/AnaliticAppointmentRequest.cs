using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.AnaliticAppointment;

public record AnaliticAppointmentRequest(Guid UserId, int FilialId): IRequest<Response>;