using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.Regions;

public class RegionsHandler: IRequestHandler<RegionsRequest, Response>
{
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public RegionsHandler(IRegionRepository regionRepository, IMapper mapper)
    {
        _regionRepository = regionRepository;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(RegionsRequest request, CancellationToken cancellationToken)
    {
        var regions = await _regionRepository.GetAllRegions();
        var resultDto = regions.Select(region => new RegionDTO(region.Id, region.Name)).ToList();
        return new Response("Regions", 200, resultDto);
    }
}