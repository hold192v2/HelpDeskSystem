using AutoMapper;
using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Interfaces;
using MassTransit;

namespace DTOs;


public class AuthCheckConsumer : IConsumer<UserCheckAuthRequestDto>
{
    private readonly IUserRepository _userRepositoryRepository;
    private readonly IRoleRepository _roleRepositoryRepository;
    private readonly IMapper _mapper;
    private readonly IRegionRepository _regionRepositoryRepository;

    public AuthCheckConsumer(IUserRepository userRepositoryRepository, IMapper mapper, IRoleRepository roleRepositoryRepository,IRegionRepository regionRepositoryRepository)
    {
        _userRepositoryRepository = userRepositoryRepository;
        _mapper = mapper;
        _roleRepositoryRepository = roleRepositoryRepository;
        _regionRepositoryRepository = regionRepositoryRepository;
    }
    public async Task Consume(ConsumeContext<UserCheckAuthRequestDto> context)
    {
        var id = context.Message.UserId;
        var user = await _userRepositoryRepository.GetUserByUserId(id);
        if (user is not null)
        {
            var role = await _roleRepositoryRepository.GetRoleNameById(user.RoleId);
            var region = await _regionRepositoryRepository.GetRegionByRegionId(user.RegionId);
        

            await context.RespondAsync(_mapper.Map<UserCheckAuthDto>
                (user, opt =>
                {
                    opt.Items["roleName"] = role;
                    opt.Items["Region"] = region;
                }));
        }
        else await context.RespondAsync(new UserCheckAuthDto());
    }
}