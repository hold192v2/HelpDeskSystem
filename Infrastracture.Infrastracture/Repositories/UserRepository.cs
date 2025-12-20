using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class UserRepository :  IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext appDbContext) 
    {
        _context = appDbContext;
    }

    public async Task<User> GetUserByUserId(Guid id)
    {
        return await _context.Users
            .Include(o => o.Offices)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<User>> GetPerformersByRegionId(int regionId, int page, int pageSize)
    {
        return await _context.Users
            .Where(user => user.RegionId == regionId && user.RoleId == 2)
            .OrderBy(user => user.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(o => o.Offices)
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByFullname(string fullname)
    {
        return _context.Users.Where(x => (x.Name + " " + x.Surname + " " + x.Patronymic).Contains(fullname)).ToList();
    }

    public async Task CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        
    }

    public async Task<bool> IsExist(Guid id)
    {
        return _context.Users.Any(x => x.Id == id);
    }
    
    public async Task<int> GetUserRegionId(Guid id)
    {
        return _context.Users.FirstOrDefaultAsync(x => x.Id == id).Result.RegionId;
    }

    public async Task<int> CountPerformersAsync() => await _context.Users.CountAsync();
}