using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.UserPanel;

public class UserPanelHandler: IRequestHandler<UserPanelRequest, Response>
{
    private readonly IUser _user;
    private readonly IRole _role;
    public UserPanelHandler(IUser user, IRole role) 
    {
        _user = user;
        _role = role;
    }
    
    public async Task<Response> Handle(UserPanelRequest request, CancellationToken cancellationToken)
    {
        var user = _user.GetUserByUserId(request.UserId);
        var role = _role.GetRoleNameByUser(user.Result);
        
        var userPanel = new UserPanelDTO();
        userPanel.Id = user.Result.Id;
        userPanel.Name = user.Result.Name;
        userPanel.Surname = user.Result.Surname;
        userPanel.Patronymic = user.Result.Patronymic;
        userPanel.Rolename = role.Result.Name;
        userPanel.Avatar = user.Result.Avatar;
        return new Response("UserPanel", 200, userPanel);
    }
}