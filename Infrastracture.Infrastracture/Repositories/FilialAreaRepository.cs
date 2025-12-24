using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class FilialAreaRepository : IFilialAreaRepository
{
    private readonly AppDbContext _context;
    
    public FilialAreaRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<string> GetFilialName(int filialId)
    {
        return (await _context.FilialAreas.FirstOrDefaultAsync(x => x.Id == filialId)).Name;
    }

    public async Task<List<FilialArea>> GetAllFilials()
    {
        return await _context.FilialAreas.OrderBy(fil => fil.Id).ToListAsync();
    }

    public async Task<FilialArea> GetFilialByFilialId(int filialId)
    {
        return await _context.FilialAreas.FirstOrDefaultAsync(x => x.Id == filialId);
    }

    public async Task UpdateAnalystId(Guid userId, int filialId)
    {
        var targetRegion = await _context.FilialAreas.FirstOrDefaultAsync(x => x.Id == filialId);
        if (targetRegion != null) targetRegion.AnaliticId = userId;
    }
}