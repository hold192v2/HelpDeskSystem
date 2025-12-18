using AutoMapper;
using DTOs;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Mappers;

public class RegisterDtoMapIntoUser : Profile
{
    public RegisterDtoMapIntoUser()
    {
        CreateMap<RegisterIntoInfrastructureDto, User>();
    }
}