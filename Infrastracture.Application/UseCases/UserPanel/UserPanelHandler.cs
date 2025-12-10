using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.UserPanel;

public class UserPanelHandler: IRequestHandler<UserPanelRequest, Response>
{
    private readonly IUser _user;
    private readonly IRole _role;
    private readonly IMapper _mapper;
    public UserPanelHandler(IUser user, IRole role, IMapper mapper) 
    {
        _user = user;
        _role = role;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(UserPanelRequest request, CancellationToken cancellationToken)
    {
        var user = _user.GetUserByUserId(request.UserId).Result;
        var role = _role.GetRoleByUser(user).Result;
        var userPanel = new UserPanelDTO();
        userPanel.Id = user.Id;
        userPanel.Name = user.Name;
        userPanel.Surname = user.Surname;
        userPanel.Patronymic = user.Patronymic;
        userPanel.Rolename = role.Name;
        userPanel.Avatar = user.Avatar;
        // var userPanel = _mapper.Map(user, new UserPanelDTO());
        
        return new Response("UserPanel", 200, userPanel);
    }
}