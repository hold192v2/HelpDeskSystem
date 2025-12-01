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
        var user = await _user.GetUserByUserId(request.UserId);
        var role = await _role.GetRoleNameByUser(user);
        if (!trueRole.Contains(role.Name))
            return new Response("", 404);
        var office = await _office.GetOfficeById(user.OfficeId);
        
        var userInfo = new UserInfoDTO();
        userInfo.Name = user.Name;
        userInfo.Surname = user.Surname;
        userInfo.Rolename = role.Name;
        userInfo.Email = user.Email;
        userInfo.Category = new List<string> {"1", "2", "3"};
        userInfo.Rating = user.Rating;
        userInfo.Office = new List<string> { office.City, office.Address };
        userInfo.RegionId = user.RegionId;
        userInfo.SystemId = user.SystemId;
        userInfo.Avatar = user.Avatar;
        
        return new Response("UserInfo", 200, userInfo);
    }
}