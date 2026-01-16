using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUserId(Guid id);
    Task<List<User>> GetPerformersWithSearchByRegionId(int regionId, int page, int pageSize, string searchString, List<Guid?> categories);
    
    Task<List<User>> GetPerformersByOfficeId(Guid officeId, Guid category);
    Task<List<User>> GetUsersByFullname(string fullname);
    public Task CreateUser(User user);
    
    Task<bool> IsExist(Guid id);
    public Task<int> GetUserRegionId(Guid id);
    Task<int> CountPerformersAsync(int regionId, string searchString);
    IQueryable<User> Query();
    Task<List<User>> GetDropDownUsers(IQueryable<User> query, string searchString);
    Task<string> GetUserName(Guid id);
}