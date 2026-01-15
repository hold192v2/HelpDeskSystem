namespace TicketService.Application.DTOs;

public record ContentTicketModel(
    Guid Id,
    string Number,
    string Theme,
    string Description,
    string Office,
    string Priority,
    string Status,
    DateTimeOffset DueAt,
    bool IsExpired);