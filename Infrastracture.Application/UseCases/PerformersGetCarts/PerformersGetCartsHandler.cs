using AutoMapper;
using Catalog.Application.DTOs;
using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MassTransit;
using MediatR;
using Response = Infrastracture.Application.HandlerResponse.Response;

namespace Infrastracture.Application.UseCases.Performers;

public class PerformersGetCartsHandler: IRequestHandler<PerformersGetCartsRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;
    private readonly IOfficeRepository _officeRepository;
    private readonly ICategoryUserRepository _categoryUserRepository;
    private readonly IRequestClient<CatalogInfoRequestDto> _requestClient;
    

    public PerformersGetCartsHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IRegionRepository regionRepository, IOfficeRepository officeRepository, ICategoryUserRepository categoryUserRepository,  IRequestClient<CatalogInfoRequestDto> requestClient)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _regionRepository = regionRepository;
        _officeRepository = officeRepository;
        _categoryUserRepository =  categoryUserRepository;
        _requestClient = requestClient;
    }
    
    public async Task<Response> Handle(PerformersGetCartsRequest getCartsRequest, CancellationToken cancellationToken)
    {
        var searchString = getCartsRequest.Fullname!.ToLower().Trim();
        var regionId = await _userRepository.GetUserRegionId((Guid)getCartsRequest.UserId!);
        var performers = await _userRepository.GetPerformersWithSearchByRegionId(regionId, getCartsRequest.Page, 20, searchString, getCartsRequest.Categories);
        var performersCount = await _userRepository.CountPerformersAsync(regionId, searchString);
        var paginationDto = new PaginationDTO(
            getCartsRequest.Page, performersCount, (performersCount + 19)/20);

        var userCategoryNames = await _categoryUserRepository.GetUserCategoriesIdListAsync(
            performers
                .Select(p => p.Id)
                .ToList());
        var allCategoryIds = userCategoryNames
            .SelectMany(x => x.Value)
            .Distinct()
            .ToList();
        var categoryNames = await _requestClient.GetResponse<CatalogInfoNameDto>(new CatalogInfoRequestDto(allCategoryIds));
            
        var contents = performers.Select(performer =>
        {
            var offices = performer.Offices.Select(office => $"{office.City} {office.Address}").ToList();
            
            userCategoryNames.TryGetValue(performer.Id, out var categoryIds);
            var names = categoryIds?
                            .Where(id => categoryNames.Message.CategoryNames.ContainsKey(id))
                            .Select(id => categoryNames.Message.CategoryNames[id])
                            .ToList()
                        ?? new List<string>();
            var rating = performer.Rating ?? 0;
            return new ContentDto(
                performer.Id,
                performer.Name,
                performer.Surname,
                performer.Patronymic,
                performer.SystemId,
                performer.Email,
                rating,
                names,  
                offices);
        }).ToList();
        
        var result = new PerformersDTO(contents, paginationDto);
        return new Response("Performers", 200, result);
    }
}