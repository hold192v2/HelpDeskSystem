using Infrastracture.Domain.Dtos;
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
        return await _context.Regions.OrderBy(reg => reg.Id).ToListAsync();
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

    public Task<Region> GetRegionByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAdminIdAsync(Guid adminId, int  regionId)
    {
        var targetRegion = await _context.Regions.FirstOrDefaultAsync(x => x.Id == regionId);
        if (targetRegion != null) targetRegion.AdminId = adminId;
    }

    public async Task<List<RegionDto>> GetRegionsWithAnalystAsync()
    {
        return await (
            from f in _context.Regions.AsNoTracking().OrderBy(reg => reg.Id)
            join u in _context.Users.AsNoTracking()
                on f.AdminId equals u.Id into users
            from u in users.DefaultIfEmpty()
            select new RegionDto(
                f.Id,
                f.Name,
                u != null ? u.Surname : null,
                u != null ? u.Name : null,
                u != null ? u.Patronymic : null
            )
        ).ToListAsync();
    }
}