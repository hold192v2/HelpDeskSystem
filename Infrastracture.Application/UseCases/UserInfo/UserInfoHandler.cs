using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.UserInfo;

public class UserInfoHandler: IRequestHandler<UserInfoRequest, Response>
{
    private readonly IUser _user;
    private readonly IRole _role;
    private readonly IOffice _office;
    public UserInfoHandler(IUser user, IRole role, IOffice office) 
    {
        _user = user;
        _role = role;
        _office = office;
    }
    
    public async Task<Response> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        var trueRole = new List<string> { "employee", "performer" };
        var user = _user.GetUserByUserId(request.UserId);
        var role = _role.GetRoleNameByUser(user.Result);
        if (!trueRole.Contains(role.Result.Name))
            return new Response("", 404);
        var office = _office.GetOfficeById(user.Result.OfficeId);
        
        var userInfo = new UserInfoDTO();
        userInfo.Name = user.Result.Name;
        userInfo.Surname = user.Result.Surname;
        userInfo.Rolename = role.Result.Name;
        userInfo.Email = user.Result.Email;
        userInfo.Category = new List<string> {"1", "2", "3"};
        userInfo.Rating = user.Result.Rating;
        userInfo.Office = new List<string> { office.Result.City, office.Result.Address };
        userInfo.RegionId = user.Result.RegionId;
        userInfo.SystemId = user.Result.SystemId;
        userInfo.Avatar = user.Result.Avatar;
        
        return new Response("UserInfo", 200, userInfo);
    }
}