namespace Ticket.Domain.Entities;

public class TicketStatusChangeLog
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public int FromStatusId { get; set; }
    public int ToStatusId { get; set; }
    public Guid ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
}