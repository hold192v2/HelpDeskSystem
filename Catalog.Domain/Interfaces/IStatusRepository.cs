namespace Catalog.Domain.Interfaces;

public interface IStatusRepository
{
    Task<List<string>> GetStatusNamesByIds(List<int> ids);
    Task<string> GetStatusNameById(int id);
}