using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.UserPanel;

public class UserPanelHandler: IRequestHandler<UserPanelRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    public UserPanelHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper) 
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(UserPanelRequest request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserByUserId(request.UserId).Result;
        var role = _roleRepository.GetRoleByUser(user).Result;
        var userPanel = new UserPanelDTO();
        userPanel.Id = user.Id;
        userPanel.Name = user.Name;
        userPanel.Surname = user.Surname;
        userPanel.Patronymic = user.Patronymic;
        userPanel.Rolename = role.Name;
        userPanel.Avatar = user.Avatar;
        
        return new Response("UserPanel", 200, userPanel);
    }
}