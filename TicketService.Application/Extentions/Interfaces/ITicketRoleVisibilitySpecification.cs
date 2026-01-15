using TicketService.Application.DTOs;
using TicketService.Application.UseCases.TicketPanel;
using TicketService.Domain.Entities;

namespace TicketService.Application.Extentions.Interfaces;

public interface ITicketRoleVisibilitySpecification
{
    IQueryable<Ticket> Apply(IQueryable<Ticket> query, TicketPanelUsabilityDto ticketContext);
}