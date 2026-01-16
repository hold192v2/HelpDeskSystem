using Catalog.Domain.Dtos;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Catalog.Infrastructure.Repositories;

public class PriorityRepository: IPriorityRepository
{
    private readonly AppDbContext _context;

    public PriorityRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Priority>> GetAllPriorities()
    {
        return await _context.Priorities.OrderBy(x => x.Id).ToListAsync();
    }

    public async Task UpdatePriority(List<PriorityUpdateDto> request)
    {
        var ids = request.Select(x => x.PriorityId).ToList();
        var priorities = await _context.Priorities
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
        var dtoDict = request.ToDictionary(x => x.PriorityId);
        foreach (var priority in priorities)
        {
            var dto = dtoDict[priority.Id];
            priority.SlaFactor = dto.Sla;
        }
         
    }

    public async Task<List<string>> GetPriorityNamesByIds(List<int> ids)
    {
        return await _context.Priorities
            .OrderBy(x => x.Id)
            .Select(priority => priority.Name)
            .ToListAsync();
    }

    public async Task<Priority> GetPriorityById(int id)
    {
        return await _context.Priorities.FirstOrDefaultAsync(x => x.Id == id); 
    }

    public async Task<string> GetPriorityNameById(int id)
    {
        var priority = await _context.Priorities.FirstOrDefaultAsync(c => c.Id == id);
        return priority!.Name;
    }

    private Task<List<Priority>> GetPriorityByIds(int[] ids)
    {
        return _context.Priorities.Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}