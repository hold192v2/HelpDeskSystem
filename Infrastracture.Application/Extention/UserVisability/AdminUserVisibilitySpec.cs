using Infrastracture.Application.Interfaces;
using Infrastracture.Application.UseCases.SearchUser;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Extention.UserVisability;

public class AdminUserVisibilitySpec : IUserRoleVisibilitySpecification
{
    public IQueryable<User> Apply(IQueryable<User> query, SearchUserRequest userContext)
    {
        return query.Where(user => user.RoleId == 1)
            .Where(user => user.RegionId == userContext.RegionId);
    }
}
