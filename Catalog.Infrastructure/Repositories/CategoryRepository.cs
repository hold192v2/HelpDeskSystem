using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public Task<List<Category>> GetDropDownCategories()
    {
        throw new NotImplementedException();
    }

    public async Task<Category> GetCategoryById(Guid id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddCategory(string name, string description, int sla)
    {
        var category = new Category();
        category.Name = name;
        category.Description = description;
        category.BasePeriodSla = sla;
        await _context.Categories.AddAsync(category);
    }

    public async Task UpdateCategory(Guid id, string name, string description, int sla)
    {
        _context.Categories.Update(new Category() 
            { Id = id, Name = name, Description = description, BasePeriodSla = sla }
        );
    }

    public async Task<Dictionary<Guid,string>> GetCategoryNames(List<Guid> categoryIds)
    {
        return await _context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .AsNoTracking()
            .Select(c => new { c.Id, c.Name })
            .ToDictionaryAsync(x => x.Id, x => x.Name);
    }

    public async Task<string> GetCategoryNameById(Guid id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return category!.Name;
    }
}