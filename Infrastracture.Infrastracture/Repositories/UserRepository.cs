using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

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
        return _context.Users.FirstOrDefault(x => x.Id == id);
    }

    public async Task<List<User>> GetUsersByRegionId(int regionId)
    {
        return _context.Users.Where(x => x.RegionId == regionId).ToList();
    }

    public async Task<List<User>> GetUsersByFullname(string fullname)
    {
        return _context.Users.Where(x => (x.Name + " " + x.Surname + " " + x.Patronymic).Contains(fullname)).ToList();
    }
}