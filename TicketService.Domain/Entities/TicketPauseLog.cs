namespace TicketService.Domain.Entities;

public class TicketPauseLog
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public int StatusId { get; set; }
    public Guid PausedByUserId { get; set; }
    public DateTime PausedAt { get; set; }
    public DateTime? ResumedAt { get; set; }
}