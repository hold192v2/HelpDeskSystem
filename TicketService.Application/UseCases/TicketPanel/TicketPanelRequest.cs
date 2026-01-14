using MediatR;
using TicketService.Application.Extentions;
using TicketService.Application.HandlerResponse;

namespace TicketService.Application.UseCases.TicketPanel;

public record TicketPanelRequest(
    int Page = 1,
    int? PriorityId = null,
    int? StatusId  = null,
    SortDirection SortByDate = SortDirection.Desc,
    string? Theme = "",
    string Role = "",
    string UserId = ""
    ) : IRequest<Response>;