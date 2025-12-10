using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.Regions;

public class RegionsHandler: IRequestHandler<RegionsRequest, Response>
{
    private readonly IRegion _region;
    private readonly IMapper _mapper;

    public RegionsHandler(IRegion region, IMapper mapper)
    {
        _region = region;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(RegionsRequest request, CancellationToken cancellationToken)
    {
        var regions = _region.GetAll();
        var result = new List<RegionDTO>();
        foreach (var region in regions)
        {
            var regionDTO = new RegionDTO();
            regionDTO.Name = region.Name;
            regionDTO.RegionId = region.Id;
            result.Add(regionDTO);
        }
        // result = regions.Select(r => _mapper.Map(r, new RegionDTO())).ToList();
        return new Response("Regions", 200, result);
    }
}