using MediatR;
using TicketService.Application.HandlerResponse;
using TicketService.Domain.Interfaces;

namespace TicketService.Application.UseCases.TicketPanel;

public class TicketPanelHandler : IRequestHandler<TicketPanelRequest, Response>
{
    private readonly ITicketRepository _ticketRepository;

    public TicketPanelHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }
    public async Task<Response> Handle(TicketPanelRequest request, CancellationToken cancellationToken)
    {
        var searchString = request.Theme!.ToLower().Trim();
        var tickets = await _ticketRepository.GetTicketsForPanelWithSearch(request.Page, 20, request.PriorityId, request.StatusId, (int)request.SortByDate, searchString);
        return new Response("OK", 200);
    }
}