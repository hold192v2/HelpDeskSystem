using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class Region: BaseRepository<Domain.Entities.Region>, IRegion
{
    private readonly AppDbContext _context;
    
    public Region(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<List<Domain.Entities.Region>> GetAllRegions()
    {
        return _context.Set<Domain.Entities.Region>().ToList();
    }

    public async Task<int> GetRegionIdByFilialId(int filialId)
    {
        return _context.Regions.FirstOrDefault(x => x.FilialId == filialId).Id;
    }
}