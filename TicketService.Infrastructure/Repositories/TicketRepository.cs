using Microsoft.EntityFrameworkCore;
using TicketService.Domain.Entities;
using TicketService.Domain.Interfaces;
using TicketService.Infrastructure.Context;

namespace TicketService.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;
    
    public TicketRepository(AppDbContext appDbContext) 
    {
        _context = appDbContext;
    }
    public Task Create(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Ticket>> GetTicketsForPanelWithSearch(IQueryable<Ticket> query, int page, int pageSize, int? priorityId, int? statusId,
        int? dateSort, string theme)
    {
        if (priorityId.HasValue)
            query = query.Where(t => t.PriorityId == priorityId.Value);
        if  (statusId.HasValue)
            query = query.Where(t => t.StatusId == statusId.Value);
        query = query.Where(t => EF.Functions.ILike(t.ThemeSearch!, $"%{theme}%"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        if (dateSort.HasValue && dateSort.Value > 0)
            query.OrderByDescending(ticket => ticket.DueAt);
        else query.OrderBy(ticket => ticket.DueAt);
        return await query.ToListAsync();
    }

    public async Task<int> CountAsync(IQueryable<Ticket> query, int? priorityId, int? statusId, int? dateSort, string theme)
    {
        if (priorityId.HasValue)
            query = query.Where(t => t.PriorityId == priorityId.Value);
        if  (statusId.HasValue)
            query = query.Where(t => t.StatusId == statusId.Value);
        query = query.Where(t => EF.Functions.ILike(t.ThemeSearch!, $"%{theme}%"));
        return await query.CountAsync();
    }

    public IQueryable<Ticket> Query()
    {
        return _context.Tickets.AsNoTracking();
    }
}