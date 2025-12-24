using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.UserInfo;

public class UserInfoHandler: IRequestHandler<UserInfoRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IRegionRepository _regionRepository;
    public UserInfoHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper,  IRegionRepository regionRepository) 
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _regionRepository = regionRepository;
    }
    
    public async Task<Response> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepository.GetUserByUserId(request.UserId);
        var role = await _roleRepository.GetRoleByUser(user);
        var regionId = await _userRepository.GetUserRegionId(request.UserId);
        var region = await _regionRepository.GetRegionByRegionId(regionId);
        
        var userInfo = _mapper.Map<UserInfoDTO>(user, opt => opt.Items["Region"] = region);
        userInfo.RoleName = role.Name;
        if (role.Name == "performer") //вырезать после добавление сервиса
        {
            userInfo.Category = new List<string> {"Транспорт"};
        }
        userInfo.Office = user.Offices
            .Select(o => $"{o.City}, {o.Address}")
            .ToList();
        if (role.Name != "performer") //вырезать после реализации норм авторизации
        {
            userInfo.Rating = null;
        }
        
        return new Response("UserInfo", 200, userInfo);
    }
}