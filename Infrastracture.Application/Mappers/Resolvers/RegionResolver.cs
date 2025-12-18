using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;

namespace Infrastracture.Application.Mappers.Resolvers;

public class RegionResolver 
    : IValueResolver<User, UserInfoDTO, string>
{
    private readonly IRegion _region;

    public RegionResolver(IRegion region)
    {
        _region = region;
    }

    public string Resolve(
        User source,
        UserInfoDTO destination,
        string destMember,
        ResolutionContext context)
    {
        return _region.GetRegionByRegionId(source.RegionId).Result.Name;
    }
}