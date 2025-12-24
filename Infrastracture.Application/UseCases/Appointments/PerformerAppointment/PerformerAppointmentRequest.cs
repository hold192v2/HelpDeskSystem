using Infrastracture.Application.HandlerResponse;
using MediatR;

namespace Infrastracture.Application.UseCases.Appointments.PerformerAppointment;

public record PerformerAppointmentRequest(Guid UserId, List<int> CategoryIds, List<Guid> OfficesIds) : IRequest<Response>;