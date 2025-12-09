using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IRole: IBaseOperationRepository<Role>
{
    Task<Role> GetRoleByUser(User user);
}