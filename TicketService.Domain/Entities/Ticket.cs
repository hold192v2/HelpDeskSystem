namespace TicketService.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public string Theme { get; set; } = "";
    public string ThemeSearch { get; set; } = "";
    public string Description { get; set; } = "";
    public string Location { get; set; } = "";
    public string Number { get; set; } = NumberGenerator();
    public Guid CategoryId { get; set; }
    public Guid OfficeId { get; set; }
    public int PriorityId { get; set; }
    public Guid CreateUserId { get; set; }
    public Guid PerformerId { get; set; } //эта
    public int StatusId { get; set; }
    public int AllocatedSeconds { get; set; } 
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset StartedAt { get; set; } // после performerId
    public DateTimeOffset DueAt { get; set; } // после performerId
    public DateTimeOffset? CompletedAt { get; set; }

    private static string NumberGenerator()
    {
        return $"ТК-{Random.Shared.Next(100000):D5}";
    }
}