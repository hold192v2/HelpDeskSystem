namespace TicketService.Application.DTOs;

public record TicketInfoCatalogRequestDto(int StatusId, int PriorityId, Guid CategoryId);