using FluentValidation;
using Infrastracture.Application.Interfaces;
using Infrastracture.Application.UseCases.SearchUser;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;

namespace Infrastracture.Application.Extention.UserVisability;

public class SuperAdminUserVisibilitySpec : IUserRoleVisibilitySpecification
{
    private readonly List<int>? _regionIds;

    public SuperAdminUserVisibilitySpec(List<int>? regionIds)
    {
        _regionIds = regionIds;
    }

    public IQueryable<User> Apply(IQueryable<User> query, SearchUserRequest context)
    {
        query = query.Where(u => u.RoleId != 5);

        if (context.RegionId.HasValue)
        {
            return query.Where(u => u.RegionId == context.RegionId);
        }

        if (_regionIds is not null)
        {
            return query.Where(u => _regionIds.Contains(u.RegionId));
        }

        throw new ValidationException("RegionId or FilialId is required");
    }
}