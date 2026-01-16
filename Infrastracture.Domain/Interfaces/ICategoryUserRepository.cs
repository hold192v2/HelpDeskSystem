namespace Infrastracture.Domain.Interfaces;

public interface ICategoryUserRepository
{
    Task<Dictionary<Guid, List<Guid>>> GetUserCategoriesIdListAsync(List<Guid> userId);
    Task SaveRelationAsync(Guid userId, List<Guid> categoryIds);
}