using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.Mappers.Resolvers;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;

namespace Infrastracture.Application.Mappers;

public class UserIntoUserInfoDto : Profile
{
    public UserIntoUserInfoDto()
    {
        CreateMap<User, UserInfoDTO>()
            .ForMember(dto => dto.Region, opt => opt.MapFrom<RegionResolver>());
    }
}