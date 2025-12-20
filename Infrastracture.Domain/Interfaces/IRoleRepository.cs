using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetRoleByUser(User user);
    Task<string> GetRoleNameById(int id);
}