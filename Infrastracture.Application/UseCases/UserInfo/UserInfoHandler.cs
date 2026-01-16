using AutoMapper;
using Catalog.Application.DTOs;
using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MassTransit;
using MediatR;
using Response = Infrastracture.Application.HandlerResponse.Response;

namespace Infrastracture.Application.UseCases.UserInfo;

public class UserInfoHandler: IRequestHandler<UserInfoRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IRegionRepository _regionRepository;
    private readonly IRequestClient<CatalogInfoRequestDto> _requestClient;
    private readonly ICategoryUserRepository _categoryUserRepository;
    public UserInfoHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper,  IRegionRepository regionRepository,  IRequestClient<CatalogInfoRequestDto> requestClient, ICategoryUserRepository categoryUserRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _regionRepository = regionRepository;
        _requestClient = requestClient;
        _categoryUserRepository =  categoryUserRepository;
    }
    
    public async Task<Response> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepository.GetUserByUserId(request.UserId);
        var role = await _roleRepository.GetRoleByUser(user);
        var regionId = await _userRepository.GetUserRegionId(request.UserId);
        var region = await _regionRepository.GetRegionByRegionId(regionId);
        
        var userInfo = _mapper.Map<UserInfoDTO>(user, opt => opt.Items["Region"] = region);
        userInfo.RoleName = role.Name;
        if (role.Name == "performer") 
        {
            var userCategoryNames = await _categoryUserRepository.GetUserCategoriesIdListAsync(new List<Guid>(){user.Id});
            var allCategoryIds = userCategoryNames
                .SelectMany(x => x.Value)
                .Distinct()
                .ToList();
            var categoryNames = await _requestClient.GetResponse<CatalogInfoNameDto>(new CatalogInfoRequestDto(allCategoryIds));
        
            userCategoryNames.TryGetValue(user.Id, out var categoryIds);
            var names = categoryIds?
                            .Where(id => categoryNames.Message.CategoryNames.ContainsKey(id))
                            .Select(id => categoryNames.Message.CategoryNames[id])
                            .ToList()
                        ?? new List<string>();
            userInfo.Category = names;
        }
        userInfo.Office = user.Offices
            .Select(o => $"{o.City}, {o.Address}")
            .ToList();
        if (role.Name != "performer") //вырезать после реализации норм авторизации
        {
            userInfo.Rating = null;
        }
        
        return new Response("UserInfo", 200, userInfo);
    }
}