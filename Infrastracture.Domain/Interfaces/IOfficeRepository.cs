using Infrastracture.Domain.Entities;
using TicketService.Application.DTOs;

namespace Infrastracture.Domain.Interfaces;

public interface IOfficeRepository
{
    Task<Office> GetOfficeByIdAsync(Guid officeId);
    Task AddOfficeAsync(string city, string address, int regionId);
    Task EditOfficeAsync(Guid officeId, string city, string adress);
    Task<List<Office>> GetOfficesByRegionIdAsync(int regionId);
    Task<List<Guid>> GetOfficesIdsByRegionIdAsync(int regionId);
    Task<List<Office>> GetOfficesByIdsAsync(IEnumerable<Guid> ids);
    Task<List<OfficeNameDto>> GetOfficesNamesByIdAsync(List<Guid> officeIds);
    Task<string> GetOfficeNameByIdAsync(Guid officeId);
}