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
        var user = _user.GetUserByUserId(request.UserId);
        var role = _role.GetRoleByUser(user.Result);
        
        var userPanel = _mapper.Map(user, new UserPanelDTO());
        userPanel.Rolename = role.Result.Name;
        return new Response("UserPanel", 200, userPanel);
    }
}