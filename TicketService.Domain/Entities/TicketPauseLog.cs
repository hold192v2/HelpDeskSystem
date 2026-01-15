namespace TicketService.Domain.Entities;

public class TicketPauseLog
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public int StatusId { get; set; }
    public Guid PausedByUserId { get; set; }
    public DateTimeOffset PausedAt { get; set; }
    public DateTimeOffset? ResumedAt { get; set; }
}