using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces;

public interface ICategoryRepository
{
    
    Task<List<Category>> GetAllCategories();
    Task<List<Category>> GetDropDownCategories();
    Task<Category> GetCategoryById(Guid id);
    Task AddCategory(string name, string description, int sla);
    Task UpdateCategory(Guid id, string name, string description, int sla);
    Task<Dictionary<Guid,string>> GetCategoryNames(List<Guid> categoryIds);
    Task<string> GetCategoryNameById(Guid id);
}