namespace TicketService.Application.DTOs;

public record CatalogTicketPanelDto(
    List<string> StatusNames, List<string> PriorityNames);