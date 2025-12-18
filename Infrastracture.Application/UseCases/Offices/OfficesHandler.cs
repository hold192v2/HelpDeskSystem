using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.Offices;

public class OfficesHandler: IRequestHandler<OfficesRequest, Response>
{
    private readonly IMapper _mapper;
    private readonly IRegion _region;
    private readonly IOffice _office;
    private readonly IUser _user;

    public OfficesHandler(IRegion region, IMapper mapper, IOffice office,  IUser user)
    {
        _region = region;
        _mapper = mapper;
        _office = office;
        _user = user;
    }
    
    public async Task<Response> Handle(OfficesRequest request, CancellationToken cancellationToken)
    {
        if (request.RegionId.HasValue && request.FillialId.HasValue)
            return new Response("Only one filter allowed", 400);
        List<Region> regions;

        if (request.RegionId.HasValue)
        {
            var region = await _region.GetRegionByRegionId(request.RegionId.Value);
            regions = new List<Region> { region };
        }
        else if (request.FillialId.HasValue)
            regions = await _region.GetRegionsByFilialId(request.FillialId.Value);
        else
        {
            var regionId = _user.GetUserRegionId(request.UserId);
            var region = await _region.GetRegionByRegionId(regionId);
            regions = new List<Region> { region };
        }
        
        var officesResult = new List<OfficeDTO>();
        foreach (var region in regions)
        {
            var offices = _office.GetOfficesByRegionId(region.Id).Result;
            foreach (var o in offices)
                officesResult.Add(new OfficeDTO(o.Id, $"{o.City} {o.Address}", o.RegionId));
        }
        return new Response("Offices", 200, officesResult);
    }
}