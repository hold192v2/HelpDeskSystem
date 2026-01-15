using TicketService.Application.DTOs;
using TicketService.Application.Extentions.ExtentionMethods;
using TicketService.Application.Extentions.Interfaces;
using TicketService.Application.UseCases.TicketPanel;
using TicketService.Domain.Entities;

namespace TicketService.Application.Extentions.TicketUsability;

public class EmployeeTicketVisibilitySpec : ITicketRoleVisibilitySpecification
{
    
    public IQueryable<Ticket> Apply(IQueryable<Ticket> query, TicketPanelUsabilityDto ticketContext)
    {
        var now = DateTime.UtcNow;
        return query.Where(ticket => ticket.CreateUserId == ticketContext.UserId)
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ThenBy(t =>
                t.StatusId == (int)StatusEnum.Completed ||
                t.StatusId == (int)StatusEnum.Rejected
                    ? 0
                    : t.DueAt < now
                        ? 1
                        : (t.DueAt - now) <= TimeSpan.FromHours(24)
                            ? 2
                            : 3
            );
    }
}