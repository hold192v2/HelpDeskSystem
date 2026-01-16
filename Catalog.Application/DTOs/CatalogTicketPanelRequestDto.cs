namespace TicketService.Application.DTOs;

public record CatalogTicketPanelRequestDto(
    List<int> StatusIds, List<int> PriorityIds);