using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class CategoryUserRepository :  ICategoryUserRepository
{
    private readonly AppDbContext _context;

    public CategoryUserRepository(AppDbContext context)
    {
        _context = context;
    }
    public Task<List<Guid>> GetUserCategoriesIdListAsync(Guid userId)
    {
        return _context.CategoryUsers
            .Where(c => c.UserId == userId)
            .Select(category => category.CategoryId)
            .ToListAsync();
    }

    public async Task<Dictionary<Guid, List<Guid>>> GetUserCategoriesIdListAsync(List<Guid> userIds)
    {
        return await _context.CategoryUsers
            .Where(cu => userIds.Contains(cu.UserId))
            .AsNoTracking()
            .GroupBy(cu => cu.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                CategoryIds = g.Select(x => x.CategoryId).ToList()
            })
            .ToDictionaryAsync(x => x.UserId, x => x.CategoryIds);
    }
    

    public async Task SaveRelationAsync(Guid userId, List<Guid> categoryIds)
    {
        var relations = categoryIds.Select(id => new CategoryUser()
        {
            UserId = userId,
            CategoryId = id
        });
        await _context.CategoryUsers.AddRangeAsync(relations);
    }
}