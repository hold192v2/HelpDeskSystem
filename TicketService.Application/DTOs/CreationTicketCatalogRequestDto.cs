namespace TicketService.Application.DTOs;

public record CreationTicketCatalogRequestDto(Guid CategoryId, int PriorityId);