namespace TicketService.Domain.Entities;

public class Report
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; } = "";
    public string Photo { get; set; } = "";
    public double Total { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}