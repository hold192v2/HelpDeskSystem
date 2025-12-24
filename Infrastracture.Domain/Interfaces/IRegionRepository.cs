using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IRegionRepository
{
    Task<List<Region>> GetAllRegions();
    Task<List<Region>> GetRegionsByFilialId(int filialId);
    Task<Region> GetRegionByRegionId(int regionId);
    Task<int> GetRegionIdByUserId(Guid userId);
    Task<Region> GetRegionByUserId(Guid userId);
    Task UpdateAdminIdAsync(Guid adminId, int regionId);
}