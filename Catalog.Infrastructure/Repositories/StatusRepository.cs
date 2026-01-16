using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class StatusRepository: IStatusRepository
{
    private readonly AppDbContext _context;
    public StatusRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<string>> GetStatusNamesByIds(List<int> ids)
    {
        return await _context.Statuses
            .OrderBy(status => status.Id)
            .Select(status => status.Code)
            .ToListAsync();
    }

    public async Task<string> GetStatusNameById(int id)
    {
        var status = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == id);
        return status!.Code;
    }
}