using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class Role: BaseRepository<Domain.Entities.Role>, IRole
{
    private readonly AppDbContext _context;
    
    public Role(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<Domain.Entities.Role?> GetRoleNameByUser(Domain.Entities.User user)
    {
        return _context.Roles.FirstOrDefault(x => x.Id == user.RoleId);
    }
}