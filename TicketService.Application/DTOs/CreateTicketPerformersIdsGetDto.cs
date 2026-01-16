namespace TicketService.Application.DTOs;

public record CreateTicketPerformersIdsGetDto(Dictionary<Guid, double?> PerformersEvaluationInfo);