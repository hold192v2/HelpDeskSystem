using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
        return await _context.Priorities.ToListAsync();
    }

    public void UpdatePriority(int id, string name, double sla)
    {
        var priority = _context.Priorities.FirstOrDefaultAsync(x => x.Id == id).Result;
        priority.Name = name;
        priority.slaFactor = sla;
    }
}