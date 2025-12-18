using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.UserInfo;

public class UserInfoHandler: IRequestHandler<UserInfoRequest, Response>
{
    private readonly IUser _user;
    private readonly IRole _role;
    private readonly IOffice _office;
    private readonly IMapper _mapper;
    private readonly IPlaceOfWork _placeOfWork;
    public UserInfoHandler(IUser user, IRole role, IOffice office, IMapper mapper, IPlaceOfWork placeOfWork) 
    {
        _user = user;
        _role = role;
        _office = office;
        _mapper = mapper;
        _placeOfWork = placeOfWork;
    }
    
    public async Task<Response> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        
        var user = await _user.GetUserByUserId(request.UserId);
        var role = await _role.GetRoleByUser(user);
        
        var userInfo = _mapper.Map(user, new  UserInfoDTO());
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