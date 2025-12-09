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
        var trueRole = new List<string> { "employee", "performer" };
        var placeOfWork = _placeOfWork.GetByUserId(request.UserId).Result;
        var user = await _user.GetUserByUserId(request.UserId);
        var role = await _role.GetRoleByUser(user);
        if (!trueRole.Contains(role.Name))
            return new Response("", 404);
        var offices = new List<string>();
        foreach (var e in placeOfWork)
        {
            var office = _office.GetOfficeById(e.OfficeId).Result;
            offices.Add($"{office.City}, {office.Address}");
        }
        
        var userInfo = _mapper.Map(user, new  UserInfoDTO());
        userInfo.Rolename = role.Name;
        userInfo.Category = new List<string> {"1", "2", "3"};
        userInfo.Office = offices;
        
        return new Response("UserInfo", 200, userInfo);
    }
}