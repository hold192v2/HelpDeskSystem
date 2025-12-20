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
    private readonly IRegionRepository _regionRepository;
    private readonly IOfficeRepository _officeRepository;
    private readonly IUserRepository _userRepository;

    public OfficesHandler(IRegionRepository regionRepository, IMapper mapper, IOfficeRepository officeRepository,  IUserRepository userRepository)
    {
        _regionRepository = regionRepository;
        _mapper = mapper;
        _officeRepository = officeRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Response> Handle(OfficesRequest request, CancellationToken cancellationToken)
    {
        if (request.RegionId.HasValue && request.FillialId.HasValue)
            return new Response("Only one filter allowed", 400);
        List<Region> regions;

        if (request.RegionId.HasValue)
        {
            var region = await _regionRepository.GetRegionByRegionId(request.RegionId.Value);
            regions = new List<Region> { region };
        }
        else if (request.FillialId.HasValue)
            regions = await _regionRepository.GetRegionsByFilialId(request.FillialId.Value);
        else
        {
            var regionId = await _userRepository.GetUserRegionId(request.UserId);
            var region = await _regionRepository.GetRegionByRegionId(regionId);
            regions = new List<Region> { region };
        }
        
        var officesResult = new List<OfficeDTO>();
        foreach (var region in regions)
        {
            var offices = _officeRepository.GetOfficesByRegionIdAsync(region.Id).Result;
            foreach (var o in offices)
                officesResult.Add(new OfficeDTO(o.Id, $"{o.City} {o.Address}", o.RegionId));
        }
        return new Response("Offices", 200, officesResult);
    }
}