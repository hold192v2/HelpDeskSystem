using MediatR;
using TicketService.Application.HandlerResponse;

namespace TicketService.Application.UseCases.TicketComments;

public record TicketCommentsRequest(Guid TicketId) : IRequest<Response>;