using TicketService.Domain.Entities;
using TicketService.Domain.Interfaces;

namespace TicketService.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    public Task Create(Domain.Entities.Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public Task<List<Ticket>> GetTicketsForPanelWithSearch(int page, int pageSize, int? priorityId, int? statusId, int? dateSort, string theme)
    {
        throw new NotImplementedException();
    }
    
    
}