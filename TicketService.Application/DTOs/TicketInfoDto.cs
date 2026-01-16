using TicketService.Domain.Entities;

namespace TicketService.Application.DTOs;

public class TicketInfoDto()
{
    public TicketInfoDto(Ticket ticket) : this()
    {
        Id = ticket.Id;
        Theme =  ticket.Theme;
        Description = ticket.Description;
        Location = ticket.Location;
        CreatedAt = ticket.CreatedAt;
        DueAt = ticket.DueAt;
        isExpired = DueAt < DateTimeOffset.UtcNow;
    }
    public Guid Id { get; set; }
    public string Theme { get; set; } = "";
    public string Description { get; set; } = "";
    public string Office { get; set; } = "";
    public string Location { get; set; } = "";
    public List<string> Photos { get; set; }
    public string Priority { get; set; } = "";
    public string Status { get; set; } = "";
    public string PerformerName { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset DueAt { get; set; }
    public bool isExpired { get; set; }
}
