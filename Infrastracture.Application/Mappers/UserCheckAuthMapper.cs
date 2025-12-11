using AutoMapper;
using DTOs;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Mappers;

public class UserCheckAuthMapper : Profile
{
    public UserCheckAuthMapper()
    {
        CreateMap<User, UserCheckAuthDto>()
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(user => user.Id))
            .ForMember(dto => dto.RoleName,
                opt => opt.MapFrom((src, dto, _, context) => context.Items["roleName"]));
    }
}