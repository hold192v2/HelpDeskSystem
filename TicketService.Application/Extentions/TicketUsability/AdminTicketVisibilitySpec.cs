using MassTransit;
using TicketService.Application.DTOs;
using TicketService.Application.Extentions.ExtentionMethods;
using TicketService.Application.Extentions.Interfaces;
using TicketService.Application.UseCases.TicketPanel;
using TicketService.Domain.Entities;

namespace TicketService.Application.Extentions.TicketUsability;

public class AdminTicketVisibilitySpec : ITicketRoleVisibilitySpecification
{
    private readonly List<Guid> _officeIds;

    public AdminTicketVisibilitySpec(List<Guid> officeIds)
    {
        _officeIds = officeIds;
    }
    public IQueryable<Ticket> Apply(IQueryable<Ticket> query, TicketPanelUsabilityDto ticketContext)
    {
        var now = DateTime.UtcNow;
        return query.Where(ticket => ticketContext.OfficeIds.Contains(ticket.OfficeId))
            .Where(ticket => ticket.CategoryId == new Guid("8c0cd55a-0661-4357-8515-24dccee23a27")) //огромный костыль для заявок "прочее"
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