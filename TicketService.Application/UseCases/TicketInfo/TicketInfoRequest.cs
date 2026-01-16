using MediatR;
using TicketService.Application.HandlerResponse;

namespace TicketService.Application.UseCases.TicketInfo;

public record TicketInfoRequest(Guid TicketId) : IRequest<Response>;