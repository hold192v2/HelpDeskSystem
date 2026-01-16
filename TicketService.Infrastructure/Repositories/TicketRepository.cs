using Microsoft.EntityFrameworkCore;
using TicketService.Application.Extentions;
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
    public async Task Create(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
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

    public async Task<Dictionary<int, List<Guid>>> PerformerTicketCount(List<Guid> performerIds)
    {
        var countsByPerformer = await _context.Tickets
            .AsNoTracking()
            .Where(t => performerIds.Contains(t.PerformerId))
            .Where(t => t.StatusId != (int)StatusEnum.Completed
                        && t.StatusId != (int)StatusEnum.Rejected)
            .GroupBy(t => t.PerformerId)
            .Select(g => new
            {
                PerformerId = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
        
        var countMap = countsByPerformer
            .ToDictionary(x => x.PerformerId, x => x.Count);
        foreach (var performerId in performerIds)
        {
            if (!countMap.ContainsKey(performerId))
                countMap[performerId] = 0;
        }
        var result = countMap
            .GroupBy(x => x.Value) 
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Key).ToList()
            );

        return result;
    }
}