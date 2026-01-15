using TicketService.Application.Extentions;

namespace TicketService.Application.DTOs;

public record TicketPanelUsabilityDto(
    int Page = 1,
    int? PriorityId = null,
    int? StatusId  = null,
    SortDirection SortByDate = SortDirection.Desc,
    string? Theme = "",
    string Role = "",
    Guid UserId = default,
    List<Guid> OfficeIds = null!
);