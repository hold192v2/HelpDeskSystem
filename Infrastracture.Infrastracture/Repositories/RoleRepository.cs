using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class RoleRepository: BaseRepository<Role>, IRole
{
    private readonly AppDbContext _context;
    
    public RoleRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<Role> GetRoleByUser(User user)
    {
        return _context.Roles.FirstOrDefault(x => x.Id == user.RoleId);
    }
}