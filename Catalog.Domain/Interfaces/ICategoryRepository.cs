using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategories();
    Task<Category> GetCategoryById(Guid id);
    Task AddCategory(string name, int sla);
    Task UpdateCategory(Guid id, string name, int sla);
}