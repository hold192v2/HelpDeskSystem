using Infrastracture.Application.DTOs;
using Infrastracture.Application.HandlerResponse;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace Infrastracture.Application.UseCases.SearchUser;

public class SearchUserHandler: IRequestHandler<SearchUserRequest, Response>
{
    private readonly IUser _user;
    private readonly IRole _role;
    private readonly IMapper _mapper;

    public SearchUserHandler(IUser user, IRole role, IMapper mapper)
    {
        _user = user;
        _role = role;
        _mapper = mapper;
    }
    
    public async Task<Response> Handle(SearchUserRequest request, CancellationToken cancellationToken)
    {
        var user = _user.GetUserByUserId((Guid)request.UserId).Result;
        var role = _role.GetRoleByUser(user).Result;
        var users = _user.GetAll().Where(u => $"{u.Name} {u.Surname} {u.Patronymic}".Contains(request.Fullname)).ToList();
        if (role.Name == "admin")
            users = users.Where(u => _role.GetRoleByUser(u).Result.Name == "performer").ToList();
        var result = users.Select(u => _mapper.Map(u, new UserDTO())).ToList();
        return new Response("Users", 200, result);
    }
}