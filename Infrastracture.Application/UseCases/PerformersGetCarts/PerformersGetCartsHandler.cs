using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.Performers;

public class PerformersGetCartsHandler: IRequestHandler<PerformersGetCartsRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;
    private readonly IOfficeRepository _officeRepository;

    public PerformersGetCartsHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IRegionRepository regionRepository, IOfficeRepository officeRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _regionRepository = regionRepository;
        _officeRepository = officeRepository;
    }
    
    public async Task<Response> Handle(PerformersGetCartsRequest getCartsRequest, CancellationToken cancellationToken)
    {
        var searchString = getCartsRequest.Fullname!.ToLower().Trim();
        var regionId = await _userRepository.GetUserRegionId((Guid)getCartsRequest.UserId!);
        var performers = await _userRepository.GetPerformersWithSearchByRegionId(regionId, getCartsRequest.Page, 20, searchString);
        var performersCount = await _userRepository.CountPerformersAsync(regionId, searchString);
        var paginationDto = new PaginationDTO(
            getCartsRequest.Page, performersCount, (performersCount + 19)/20);

        var contents = performers.Select(performer =>
        {
            var offices = performer.Offices.Select(office => $"{office.City} {office.Address}").ToList();
            return new ContentDto(
                performer.Id,
                performer.Name,
                performer.Surname,
                performer.Patronymic,
                performer.SystemId,
                performer.Email,
                (double)performer.Rating!,
                ["Транспорт", "Переезд"],  offices); // TODO переделать при создании баз с категориями
        }).ToList();
        
        var result = new PerformersDTO(contents, paginationDto);
        return new Response("Performers", 200, result);
    }
}