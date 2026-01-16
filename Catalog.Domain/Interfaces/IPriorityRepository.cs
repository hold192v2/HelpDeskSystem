using Catalog.Domain.Dtos;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces;

public interface IPriorityRepository
{
    Task<List<Priority>> GetAllPriorities();
    Task UpdatePriority(List<PriorityUpdateDto> request);
    Task<List<string>> GetPriorityNamesByIds(List<int> ids);
    Task<Priority> GetPriorityById(int id);
    Task<string> GetPriorityNameById(int id);
}