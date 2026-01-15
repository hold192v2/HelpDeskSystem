namespace TicketService.Domain.Entities;

public class Feedback
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid CreateUserId { get; set; }
    public int Rating { get; set; }
    public string? Message { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}