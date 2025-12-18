using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IOffice: IBaseOperationRepository<Office>
{
    Task<Office> GetOfficeById(Guid officeId);
    void AddOffice(string city, string address, int regionId);
    void EditOffice(Guid officeId, string city, string adress);
    Task<List<Office>> GetOfficesByRegionId(int regionId);
}