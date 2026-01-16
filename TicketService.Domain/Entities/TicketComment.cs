namespace TicketService.Domain.Entities;

public class TicketComment
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid UserId { get; set; }
    public string? Message { get; set; }
    public DateTimeOffset CreatedAt { get; set; } =  DateTimeOffset.UtcNow;
}