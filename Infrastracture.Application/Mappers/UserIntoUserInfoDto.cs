using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Mappers;

public class UserIntoUserInfoDto : Profile
{
    public UserIntoUserInfoDto()
    {
        CreateMap<User, UserInfoDTO>();
    }
}