using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.Regions;

public class RegionsHandler: IRequestHandler<RegionsRequest, Response>
{
    private readonly IRegionRepository _regionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegionsHandler(IRegionRepository regionRepository, IMapper mapper,  IUserRepository userRepository)
    {
        _regionRepository = regionRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<Response> Handle(RegionsRequest request, CancellationToken cancellationToken)
    {
        var regions = await _regionRepository.GetRegionsWithAnalystAsync();

        return new Response("Regions", 200, regions);
    }
}