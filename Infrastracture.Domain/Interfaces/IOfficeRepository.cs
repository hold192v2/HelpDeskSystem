using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IOfficeRepository
{
    Task<Office> GetOfficeByIdAsync(Guid officeId);
    Task AddOfficeAsync(string city, string address, int regionId);
    Task EditOfficeAsync(Guid officeId, string city, string adress);
    Task<List<Office>> GetOfficesByRegionIdAsync(int regionId);
}