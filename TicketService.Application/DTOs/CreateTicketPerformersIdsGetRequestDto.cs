namespace TicketService.Application.DTOs;

public record CreateTicketPerformersIdsGetRequestDto(Guid OfficeId, Guid CategoryId);