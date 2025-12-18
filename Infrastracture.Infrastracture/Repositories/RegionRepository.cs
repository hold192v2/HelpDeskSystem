using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class RegionRepository: BaseRepository<Region>, IRegion
{
    private readonly AppDbContext _context;
    
    public RegionRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<List<Region>> GetAllRegions()
    {
        return  _context.Set<Region>().ToList();
    }

    public async Task<List<Region>> GetRegionsByFilialId(int filialId)
    {
        return _context.Regions.Where(x => x.FilialId == filialId).ToList();
    }

    public async Task<Region> GetRegionByRegionId(int regionId)
    {
        return _context.Regions.FirstOrDefault(x => x.Id == regionId);
    }

    public async Task<int> GetRegionIdByUserId(Guid userId)
    {
         return _context.Regions.FirstOrDefault(x => x.AdminId == userId).Id;
    }
}