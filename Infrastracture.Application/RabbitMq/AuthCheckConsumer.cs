using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Interfaces;
using MassTransit;

namespace DTOs;


public class AuthCheckConsumer : IConsumer<UserCheckAuthRequestDto>
{
    private readonly IUser _userRepository;
    private readonly IRole _roleRepository;
    private readonly IMapper _mapper;

    public AuthCheckConsumer(IUser userRepository, IMapper mapper, IRole roleRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    public async Task Consume(ConsumeContext<UserCheckAuthRequestDto> context)
    {
        var id = context.Message.UserId;
        var user = await _userRepository.GetUserByUserId(id);
        if (user is not null)
        {
            var role = await _roleRepository.GetRoleNameById(user.RoleId);
            await context.RespondAsync(_mapper.Map<UserCheckAuthDto>
                (user, opt => opt.Items["roleName"] = role));
        }
        else await context.RespondAsync(new UserCheckAuthDto());
    }
}