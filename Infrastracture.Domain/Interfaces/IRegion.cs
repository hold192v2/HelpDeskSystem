using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IRegion: IBaseOperationRepository<Region>
{
    Task<List<Region>> GetAllRegions();
    Task<List<Region>> GetRegionsByFilialId(int filialId);
    Task<Region> GetRegionByRegionId(int regionId);
    Task<int> GetRegionIdByUserId(Guid userId);
}