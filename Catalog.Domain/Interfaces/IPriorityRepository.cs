using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces;

public interface IPriorityRepository
{
    Task<List<Priority>> GetAllPriorities();
    void UpdatePriority(int id, string name, double sla);
}