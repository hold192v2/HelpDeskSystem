using System.ComponentModel;
using Infrastracture.Application.DTOs;
using Infrastracture.Application.Extention;
using Infrastracture.Application.Extention.UserVisability;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Application.Interfaces;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace Infrastracture.Application.UseCases.SearchUser;

public class SearchUserHandler: IRequestHandler<SearchUserRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public SearchUserHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IRegionRepository regionRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _regionRepository = regionRepository;
    }
    
    public async Task<Response> Handle(SearchUserRequest request, CancellationToken cancellationToken)
    {
        var regionsId = new List<int>();
        if (request.FilialId.HasValue)
        {
           regionsId = (await _regionRepository
               .GetRegionsByFilialId(request.FilialId.Value))
               .Select(r => r.Id)
               .ToList();
        }
        else if (request.RegionId.HasValue) regionsId.Add(request.RegionId.Value);
        else return new Response("Incorrect request", 404);
        if (request.Role == null) return new Response("Incorrect request, role is not specified.", 404);
        var specification = Resolve(request.Role,  regionsId);
        var baseQuery = _userRepository.Query();

        baseQuery = specification.Apply(baseQuery, request);

        var users = await _userRepository.GetDropDownUsers(
            baseQuery,
            request.Search!
        );
        var result = users.Select(user => 
            new DropDownUserDto(user.Id, user.Name, user.Surname, user.Patronymic, user.Email, user.Rating)).ToList();
        return new Response("Users", 200, result);
    }
    
    private IUserRoleVisibilitySpecification Resolve(string role, List<int> regionIds)
    {
        return role switch
        {
            "admin" => new AdminUserVisibilitySpec(),
            "superadmin" => new SuperAdminUserVisibilitySpec(regionIds),
            _ => throw new InvalidEnumArgumentException()
        };
    }
    
}