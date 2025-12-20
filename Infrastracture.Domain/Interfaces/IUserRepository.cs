using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUserId(Guid id);
    Task<List<User>> GetPerformersByRegionId(int regionId, int page, int pageSize);
    Task<List<User>> GetUsersByFullname(string fullname);
    public Task CreateUser(User user);
    
    Task<bool> IsExist(Guid id);
    public Task<int> GetUserRegionId(Guid id);
    public Task<int> CountPerformersAsync();
}