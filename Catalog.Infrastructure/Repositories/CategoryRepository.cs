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

    public async Task<Category> GetCategoryById(Guid id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddCategory(string name, int sla)
    {
        var category = new Category();
        category.Name = name;
        category.BasePeriodSla = sla;
        await _context.Categories.AddAsync(category);
    }

    public async Task UpdateCategory(Guid id, string name, int sla)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);;
        category.Name = name;
        category.BasePeriodSla = sla;
        await _context.SaveChangesAsync();
    }
}