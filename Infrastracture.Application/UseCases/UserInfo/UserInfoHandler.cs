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
    private readonly IMapper _mapper;
    public UserInfoHandler(IUser user, IRole role, IOffice office, IMapper mapper) 
    {
        _user = user;
        _role = role;
        _office = office;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        var trueRole = new List<string> { "employee", "performer" };
        var user = await _user.GetUserByUserId(request.UserId);
        var role = await _role.GetRoleNameByUser(user);
        if (!trueRole.Contains(role.Name))
            return new Response("", 404);
        var office = await _office.GetOfficeById(user.OfficeId);
        
        var userInfo = _mapper.Map(user, new  UserInfoDTO());
        userInfo.Rolename = role.Name;
        userInfo.Category = new List<string> {"1", "2", "3"};
        userInfo.Office = new List<string> { $"{office.City}, {office.Address}"};
        
        return new Response("UserInfo", 200, userInfo);
    }
}