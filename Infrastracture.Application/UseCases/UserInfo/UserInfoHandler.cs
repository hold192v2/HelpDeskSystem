using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.UserInfo;

public class UserInfoHandler: IRequestHandler<UserInfoRequest, Response>
{
    private readonly IUserRepository _userRepositoryRepository;
    private readonly IRoleRepository _roleRepositoryRepository;
    private readonly IMapper _mapper;
    private readonly IRegionRepository _regionRepositoryRepository;
    public UserInfoHandler(IUserRepository userRepositoryRepository, IRoleRepository roleRepositoryRepository, IMapper mapper,  IRegionRepository regionRepositoryRepository) 
    {
        _userRepositoryRepository = userRepositoryRepository;
        _roleRepositoryRepository = roleRepositoryRepository;
        _mapper = mapper;
        _regionRepositoryRepository = regionRepositoryRepository;
    }
    
    public async Task<Response> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepositoryRepository.GetUserByUserId(request.UserId);
        var role = await _roleRepositoryRepository.GetRoleByUser(user);
        var region = await _regionRepositoryRepository.GetRegionIdByUserId(request.UserId);
        
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