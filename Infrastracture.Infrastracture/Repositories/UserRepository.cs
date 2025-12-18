using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class UserRepository: BaseRepository<User>, IUser
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<User> GetUserByUserId(Guid id)
    {
        return await _context.Users
            .Include(o => o.Offices)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<User>> GetUsersByRegionId(int regionId)
    {
        return _context.Users.Include(o => o.Offices).Where(x => x.RegionId == regionId).ToList();
    }

    public async Task<List<User>> GetUsersByFullname(string fullname)
    {
        return _context.Users.Where(x => (x.Name + " " + x.Surname + " " + x.Patronymic).Contains(fullname)).ToList();
    }

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
    }

    public async Task<bool> IsExist(Guid id)
    {
        return _context.Users.Any(x => x.Id == id);
    }
    
    public int GetUserRegionId(Guid id)
    {
        return _context.Users.FirstOrDefaultAsync(x => x.Id == id).Result.RegionId;
    }
}