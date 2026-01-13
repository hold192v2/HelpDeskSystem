namespace Ticket.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public string Theme { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Photo {get; set;}
    public string Location { get; set; } = "";
    public Guid CategoryId { get; set; }
    public Guid OfficeId { get; set; }
    public int PriorityId { get; set; }
    public Guid CreateUserId { get; set; }
    public Guid PerformerId { get; set; }
    public int StatusId { get; set; }
    public int AllocatedSeconds { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}