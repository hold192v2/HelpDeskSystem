using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.AnaliticAppointment;

public class AnaliticAppointmentRequest(Guid UserId, int FilialId): IRequest<Response>;