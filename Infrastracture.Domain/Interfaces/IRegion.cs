using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IRegion: IBaseOperationRepository<Region>
{
    Task<List<Region>> GetAllRegions();
    Task<int> GetRegionIdByFilialId(int filialId);
}