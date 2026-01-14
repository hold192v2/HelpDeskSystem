using TicketService.Application.Extentions.Interfaces;
using TicketService.Application.UseCases.TicketPanel;
using TicketService.Domain.Entities;

namespace TicketService.Application.Extentions.TicketUsability;

public class AdminTicketVisibilitySpec : ITicketRoleVisibilitySpecification
{
    public IQueryable<Ticket> Apply(IQueryable<Ticket> query, TicketPanelRequest ticketContext)
    {
        throw new NotImplementedException();
    }
}