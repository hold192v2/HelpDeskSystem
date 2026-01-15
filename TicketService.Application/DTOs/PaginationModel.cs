namespace TicketService.Application.DTOs;

public record PaginationModel(int PageIndex, int TotalRecords, int TotalPages);