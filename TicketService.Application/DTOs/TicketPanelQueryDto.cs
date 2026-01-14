using TicketService.Application.Extentions;

namespace TicketService.Application.DTOs;

public record TicketPanelQueryDto(
    int Page = 1,
    int? PriorityId = null,
    int? StatusId  = null,
    SortDirection SortByDate = SortDirection.Desc,
    string? Theme = "");