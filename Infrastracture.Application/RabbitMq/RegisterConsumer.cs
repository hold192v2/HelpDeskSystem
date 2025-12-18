using AutoMapper;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MassTransit;

namespace DTOs;

public class RegisterConsumer : IConsumer<RegisterIntoInfrastructureDto>
{
    private readonly IUser _userRepository;
    private readonly IRole _roleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterConsumer(IUser userRepository, IMapper mapper, IRole roleRepository,  IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext<RegisterIntoInfrastructureDto> context)
    {
        var user = _mapper.Map<User>(context.Message);
        if (user == null && !(await _userRepository.IsExist(user.Id)))
        {
            _userRepository.CreateUser(user!);
            await context.RespondAsync(_mapper.Map<UserCheckAuthDto>(user));
        }
        else await context.RespondAsync(new UserCheckAuthDto());
        await _unitOfWork.Commit(new CancellationToken());
    }
}