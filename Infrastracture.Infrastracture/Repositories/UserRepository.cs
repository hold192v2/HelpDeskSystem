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

    public async Task<List<User>> GetPerformersWithSearchByRegionId(int regionId, int page, int pageSize, string searchString)
    {
        var baseQuery = _context.Users
            .Where(u => u.RegionId == regionId)
            .Where(u => u.RoleId == 2)
            .Where(u => EF.Functions.ILike(u.FullName!, $"%{searchString}%"))
            .AsNoTracking();

        var users = await baseQuery
            .OrderBy(u => u.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(u => u.Offices)
            .ToListAsync();

        return users;
    }

    public async Task<List<User>> GetUsersByFullname(string fullname)
    {
        return await _context.Users.Where(x => (x.Name + " " + x.Surname + " " + x.Patronymic).Contains(fullname)).ToListAsync();
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

    public async Task<int> CountPerformersAsync(int regionId, string searchString)
    {
        return await _context.Users
            .Where(u => u.RegionId == regionId)
            .Where(u => u.RoleId == 2)
            .Where(u => EF.Functions.ILike(u.FullName!, $"%{searchString}%"))
            .AsNoTracking()
            .CountAsync();
    } 
    public IQueryable<User> Query()
    {
        return _context.Users.AsNoTracking();
    }

    public async Task<List<User>> GetDropDownUsers(IQueryable<User> query, string searchString)
    {
        return await query
            .Where(u => EF.Functions.ILike(u.FullName!, $"%{searchString}%"))
            .OrderBy(u => u.UpdatedAt)
            .Take(20)
            .ToListAsync();
    }
}