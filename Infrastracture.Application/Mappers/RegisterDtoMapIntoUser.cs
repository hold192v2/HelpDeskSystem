using AutoMapper;
using DTOs;
using Infrastracture.Application.Mappers.Resolvers;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Mappers;

public class RegisterDtoMapIntoUser : Profile
{
    public RegisterDtoMapIntoUser()
    {
        CreateMap<RegisterIntoInfrastructureDto, User>()
            .ForMember(user => user.Offices, opt => opt.MapFrom<OfficesRegisterResolver>())
            .ForMember(user => user.CreatedAt,
                opt => opt.MapFrom((src, dto, _, context) => context.Items["CreatedAt"]))
            .ForMember(user => user.UpdatedAt,
                opt => opt.MapFrom((src, dto, _, context) => context.Items["UpdatedAt"]))
            .ForMember(user => user.FullName,
                opt => opt.MapFrom(dto => $"{dto.Surname} {dto.Name} {dto.Patronymic}".Trim().ToLower()));
    }
}