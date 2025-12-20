using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class RoleRepository :  IRoleRepository
{
    private readonly AppDbContext _context;
    
    public RoleRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<Role> GetRoleByUser(User user)
    {
        return _context.Roles.FirstOrDefault(x => x.Id == user.RoleId);
    }
    public Task<string> GetRoleNameById(int id)
    {
        var role = _context.Roles.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(role.Name)!;
    }
}