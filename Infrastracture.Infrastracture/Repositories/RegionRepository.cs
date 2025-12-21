using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class RegionRepository :  IRegionRepository
{
    private readonly AppDbContext _context;
    
    public RegionRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<List<Region>> GetAllRegions()
    {
        return await _context.Regions.ToListAsync();
    }

    public async Task<List<Region>> GetRegionsByFilialId(int filialId)
    {
        return await _context.Regions.Where(x => x.FilialId == filialId).ToListAsync();
    }

    public async Task<Region> GetRegionByRegionId(int regionId)
    {
        return await _context.Regions.FirstOrDefaultAsync(x => x.Id == regionId);
    }

    public async Task<int> GetRegionIdByUserId(Guid userId)
    {
         return _context.Regions.FirstOrDefaultAsync(x => x.AdminId == userId).Id;
    }
}