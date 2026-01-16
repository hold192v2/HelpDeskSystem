namespace TicketService.Application.DTOs;

public record CreateQueryDto(
    string Theme, 
    string Description, 
    List<string> PhotosUrl, 
    Guid OfficeId, 
    int PriorityId,
    Guid CategoryId, 
    string Location);