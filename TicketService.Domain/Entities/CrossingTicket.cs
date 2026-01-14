namespace TicketService.Domain.Entities;

public class CrossingTicket : Ticket
{
    public Guid OfficeWhereId {get; set;}
}