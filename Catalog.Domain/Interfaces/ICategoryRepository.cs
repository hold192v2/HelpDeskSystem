using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategories();
    Task<Category> GetCategoryById(Guid id);
    void AddCategory(string name, int sla);
    void UpdateCategory(Guid id, string name, int sla);
}