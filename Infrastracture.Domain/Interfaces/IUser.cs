using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IUser: IBaseOperationRepository<User>
{
    Task<User> GetUserByUserId(Guid id);
}