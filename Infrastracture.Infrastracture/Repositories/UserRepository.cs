using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class UserRepository: BaseRepository<Domain.Entities.User>, IUser
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<Domain.Entities.User?> GetUserByUserId(Guid id)
    {
        return _context.Users.FirstOrDefault(x => x.Id == id);
    }

    public async Task<List<User>> GetUsersByRegionId(int regionId)
    {
        return _context.Users.Where(x => x.RegionId == regionId).ToList();
    }
}