using AutoMapper;
using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using MassTransit;

namespace DTOs;

public class RegisterConsumer : IConsumer<RegisterIntoInfrastructureDto>
{
    private readonly IUserRepository _userRepositoryRepository;
    private readonly IRoleRepository _roleRepositoryRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOfficeRepository _officeRepositoryRepository;

    public RegisterConsumer(IUserRepository userRepositoryRepository, IMapper mapper, IRoleRepository roleRepositoryRepository,  IUnitOfWork unitOfWork, IOfficeRepository officeRepositoryRepository)
    {
        _userRepositoryRepository = userRepositoryRepository;
        _mapper = mapper;
        _roleRepositoryRepository = roleRepositoryRepository;
        _unitOfWork = unitOfWork;
        _officeRepositoryRepository = officeRepositoryRepository;
        
    }
    public async Task Consume(ConsumeContext<RegisterIntoInfrastructureDto> context)
    {
        var office = await _officeRepositoryRepository.GetOfficeByIdAsync(context.Message.OfficeId);
        var user = _mapper.Map<User>(context.Message, opt =>
        {
            opt.Items["Office"] = office;
            opt.Items["CreatedAt"] = DateTime.UtcNow;
            opt.Items["UpdatedAt"] = DateTime.UtcNow;
        });
        
        if (user != null && !(await _userRepositoryRepository.IsExist(user.Id)))
        {
            var role = await _roleRepositoryRepository.GetRoleNameById(user.RoleId);
            await _userRepositoryRepository.CreateUser(user!);
            await context.RespondAsync(_mapper.Map<UserCheckAuthDto>(user, opt =>
            {
                opt.Items["roleName"] = role;
            }));
        }
        else await context.RespondAsync(new UserCheckAuthDto());
        await _unitOfWork.Commit(new CancellationToken());
    }
}