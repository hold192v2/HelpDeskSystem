using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Application.Mappers.Resolvers;

public class RegionResolver : IValueResolver<User, UserInfoDTO, string>
{
    public string Resolve(User source, UserInfoDTO destination, string destMember, ResolutionContext context)
    {
        var region = (Region)context.Items["Region"];
        return region.Name;
    }
}