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

    public OfficesHandler(IRegion region, IMapper mapper, IOffice office)
    {
        _region = region;
        _mapper = mapper;
        _office = office;
    }
    
    public async Task<Response> Handle(OfficesRequest request, CancellationToken cancellationToken)
    {
        var regions = new List<Region>();
        if (request.RegioId != null)
            regions.Add(_region.GetRegionByRegionId((int)request.RegioId).Result);
        else
            if (request.FillialId != null)
                regions = _region.GetRegionsByFilialId((int)request.FillialId).Result;
        
        var officesResult = new List<OfficeDTO>();
        foreach (var region in regions)
        {
            var offices = _office.GetOfficesByRegionId(region.Id).Result;
            officesResult.AddRange(offices.Select(office => _mapper.Map(office, new OfficeDTO())));
        }
        return new Response("Offices", 200, officesResult);
    }
}