using Infrastracture.Application.Extention;
using Infrastracture.Application.UseCases.SearchUser;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Interfaces;

public interface IUserRoleVisibilitySpecification
{
    IQueryable<User> Apply(IQueryable<User> query, SearchUserRequest userContext);
}