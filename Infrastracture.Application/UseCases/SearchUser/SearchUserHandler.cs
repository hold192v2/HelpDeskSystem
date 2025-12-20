using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace Infrastracture.Application.UseCases.SearchUser;

public class SearchUserHandler: IRequestHandler<SearchUserRequest, Response>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public SearchUserHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(SearchUserRequest request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserByUserId((Guid)request.UserId).Result;
        var role = _roleRepository.GetRoleByUser(user).Result;
        var users = _userRepository.GetUsersByFullname(request.Fullname).Result;
        if (role.Name == "admin")
            users = users.Where(u => _roleRepository.GetRoleByUser(u).Result.Name == "performer").ToList();
        // var result = users.Select(u => _mapper.Map(u, new UserDTO())).ToList();
        var result = new List<UserDTO>();
        foreach (var u in users)
        {
            var userDTO = new UserDTO();
            userDTO.UserId = u.Id;
            userDTO.Fullname = $"{u.Name} {u.Surname} {u.Patronymic}";
            result.Add(userDTO);
        }
        return new Response("Users", 200, result);
    }
}