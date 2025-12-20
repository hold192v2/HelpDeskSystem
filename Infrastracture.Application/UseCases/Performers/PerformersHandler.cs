using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;

namespace Infrastracture.Application.UseCases.Performers;

public class PerformersHandler: IRequestHandler<PerformersRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;
    private readonly IOfficeRepository _officeRepository;

    public PerformersHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IRegionRepository regionRepository, IOfficeRepository officeRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _regionRepository = regionRepository;
        _officeRepository = officeRepository;
    }
    
    public async Task<Response> Handle(PerformersRequest request, CancellationToken cancellationToken)
    {
        var performerCount = await _userRepository.CountPerformersAsync();
        
        var regionId = await _regionRepository.GetRegionIdByUserId((Guid)request.UserId!);
        var performers = await _userRepository.GetPerformersByRegionId(regionId, request.Page, 20);
        
        if (request.Fullname != null)
            performers = performers.Where(p => $"{p.Name} {p.Surname} {p.Patronymic}".Contains(request.Fullname)).ToList();
        if (request.OfficeIds.Any())
        {
            performers = performers
                .Where(user => user.Offices.Any(office => request.OfficeIds.Contains(office.Id)))
                .ToList();
        }
            
        
        var paginationDTO = new PaginationDTO(
            request.Page, performerCount, (performerCount + 19)/20);

        var contents = performers.Select(performer =>
        {
            var offices = performer.Offices.Select(office => $"{office.City} {office.Address}").ToList();
            return new ContentDto(performer.Id, performer.Name, performer.Surname, performer.Patronymic, [$"{performer.CategoryId}"],  offices);
        });
        
        var result = new PerformersDTO();
        result.Pagination = paginationDTO;
        result.Content = contents.ToList();
        return new Response("Performers", 200, result);
    }
}