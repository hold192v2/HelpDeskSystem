using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IUser: IBaseOperationRepository<User>
{
    Task<User> GetUserByUserId(Guid id);
    Task<List<User>> GetUsersByRegionId(int regionId);
    Task<List<User>> GetUsersByFullname(string fullname);
}