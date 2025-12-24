using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Interfaces;
using MassTransit;
using MediatR;
using Response = Infrastracture.Application.HandlerResponse.Response;

namespace Infrastracture.Application.UseCases.AdminAppointment;

public class AdminAppointmentHandler : IRequestHandler<AdminAppointmentRequest, Response>
{
    private readonly IRequestClient<ChangeRoleRequestDto> _requestClient;
    private readonly IUserRepository _userRepository;
    private readonly IOfficeRepository _officeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegionRepository _regionRepository;

    public AdminAppointmentHandler(IRequestClient<ChangeRoleRequestDto> requestClient, IUserRepository userRepository,
        IOfficeRepository officeRepository, IUnitOfWork unitOfWork, IRegionRepository regionRepository)
    {
        _requestClient = requestClient;
        _userRepository = userRepository;
        _officeRepository = officeRepository;
        _unitOfWork = unitOfWork;
        _regionRepository = regionRepository;
    }

    public async Task<Response> Handle(AdminAppointmentRequest request, CancellationToken cancellationToken)
    {
        var adminId = (await _regionRepository.GetRegionByRegionId(request.RegionId)).AdminId;
        if (adminId != null)
        {
            var isSuccessfulChangeRoleFromAdminToEmployee = await _requestClient.GetResponse<ChangeRoleResponse>(new ChangeRoleRequestDto()
            {
                RoleNameAssign = "employee",
                RolesNameDelete = new List<string>{"admin"},
                UserId = adminId.Value
            });
            if (isSuccessfulChangeRoleFromAdminToEmployee.Message.IsSuccessful)
            {
                var user = await _userRepository.GetUserByUserId(adminId.Value);
                user.RoleId = 1;
                user.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Commit(new CancellationToken());
                var isSuccessfulChangeRoleToAdmin = await _requestClient.GetResponse<ChangeRoleResponse>(new ChangeRoleRequestDto()
                {
                    RoleNameAssign = "admin",
                    RolesNameDelete = new List<string>{"employee","performer", "analyst"},
                    UserId = request.UserId
                });
                if (isSuccessfulChangeRoleToAdmin.Message.IsSuccessful)
                {
                    var newAdmin = await _userRepository.GetUserByUserId(request.UserId);
                    newAdmin.RoleId = 3;
                    newAdmin.UpdatedAt = DateTime.UtcNow;
                    await _regionRepository.UpdateAdminIdAsync(request.UserId, request.RegionId);
                    await _unitOfWork.Commit(new CancellationToken());
                
                    return new Response("The role is changed succesfully", 200);
                }
                else return new Response("Something wrong when tried to change role", 404);
            }
            else return new Response("Something wrong when tried to change role", 404);
        }
        else
        {
            var isSuccessfulChangeRoleToAdmin = await _requestClient.GetResponse<ChangeRoleResponse>(new ChangeRoleRequestDto()
            {
                RoleNameAssign = "admin",
                RolesNameDelete = new List<string>{"employee","performer","analyst"},
                UserId = request.UserId
            });
            if (isSuccessfulChangeRoleToAdmin.Message.IsSuccessful)
            {
                var newAdmin = await _userRepository.GetUserByUserId(request.UserId);
                newAdmin.RoleId = 3;
                newAdmin.UpdatedAt = DateTime.UtcNow;
                await _regionRepository.UpdateAdminIdAsync(request.UserId, request.RegionId);
                await _unitOfWork.Commit(new CancellationToken());
                
                return new Response("The role is changed succesfully", 200);
            }
            else return new Response("Something wrong when tried to change role", 404);
        }
        
    }
}