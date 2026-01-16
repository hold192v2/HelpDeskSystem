namespace TicketService.Domain.Entities;

public class PhotoConnection
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    
    public string PhotoUrl { get; set; } = "";
}