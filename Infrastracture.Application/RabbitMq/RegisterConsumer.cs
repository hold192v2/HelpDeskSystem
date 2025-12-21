using AutoMapper;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MassTransit;

namespace DTOs;

public class RegisterConsumer : IConsumer<RegisterIntoInfrastructureDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOfficeRepository _officeRepository;

    public RegisterConsumer(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository,  IUnitOfWork unitOfWork, IOfficeRepository officeRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _officeRepository = officeRepository;
        
    }
    public async Task Consume(ConsumeContext<RegisterIntoInfrastructureDto> context)
    {
        var office = await _officeRepository.GetOfficeByIdAsync(context.Message.OfficeId);
        var user = _mapper.Map<User>(context.Message, opt =>
        {
            opt.Items["Office"] = office;
            opt.Items["CreatedAt"] = DateTime.UtcNow;
            opt.Items["UpdatedAt"] = DateTime.UtcNow;
        });
        
        if (user != null && !(await _userRepository.IsExist(user.Id)))
        {
            var role = await _roleRepository.GetRoleNameById(user.RoleId);
            await _userRepository.CreateUser(user!);
            await context.RespondAsync(_mapper.Map<UserCheckAuthDto>(user, opt =>
            {
                opt.Items["roleName"] = role;
            }));
        }
        else await context.RespondAsync(new UserCheckAuthDto());
        await _unitOfWork.Commit(new CancellationToken());
    }
}