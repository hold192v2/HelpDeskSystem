using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Interfaces;
using MassTransit;
using MediatR;
using Response = Infrastracture.Application.HandlerResponse.Response;

namespace Infrastracture.Application.UseCases.AnaliticAppointment;

public class AnaliticAppointmentHandler : IRequestHandler<AnaliticAppointmentRequest, Response>
{
    private readonly IRequestClient<ChangeRoleRequestDto> _requestClient;
    private readonly IUserRepository _userRepository;
    private readonly IOfficeRepository _officeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegionRepository _regionRepository;
    private readonly IFilialAreaRepository _filialAreaRepository;
    
    public AnaliticAppointmentHandler(IRequestClient<ChangeRoleRequestDto> requestClient, IUserRepository userRepository,
        IOfficeRepository officeRepository, IUnitOfWork unitOfWork, IRegionRepository regionRepository,  IFilialAreaRepository filialAreaRepository)
    {
        _requestClient = requestClient;
        _userRepository = userRepository;
        _officeRepository = officeRepository;
        _unitOfWork = unitOfWork;
        _regionRepository = regionRepository;
        _filialAreaRepository = filialAreaRepository;
    }
    public async Task<Response> Handle(AnaliticAppointmentRequest request, CancellationToken cancellationToken)
    {
        var analystId = (await _filialAreaRepository.GetFilialByFilialId(request.FilialId)).AnaliticId;
        if (analystId != null)
        {
            var isSuccessfulChangeRoleFromAdminToEmployee = await _requestClient.GetResponse<ChangeRoleResponse>(new ChangeRoleRequestDto()
            {
                RoleNameAssign = "employee",
                RolesNameDelete = new List<string>{"analyst"},
                UserId = analystId.Value
            });
            if (isSuccessfulChangeRoleFromAdminToEmployee.Message.IsSuccessful)
            {
                var user = await _userRepository.GetUserByUserId(analystId.Value);
                user.RoleId = 1;
                user.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Commit(new CancellationToken());
                var isSuccessfulChangeRoleToAdmin = await _requestClient.GetResponse<ChangeRoleResponse>(new ChangeRoleRequestDto()
                {
                    RoleNameAssign = "analyst",
                    RolesNameDelete = new List<string>{"employee","performer","admin"},
                    UserId = request.UserId
                });
                if (isSuccessfulChangeRoleToAdmin.Message.IsSuccessful)
                {
                    var newAnalyst = await _userRepository.GetUserByUserId(request.UserId);
                    newAnalyst.RoleId = 4;
                    newAnalyst.UpdatedAt = DateTime.UtcNow;
                    await _filialAreaRepository.UpdateAnalystId(request.UserId, request.FilialId);
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
                RoleNameAssign = "analyst",
                RolesNameDelete = new List<string>{"employee","performer","admin"},
                UserId = request.UserId
            });
            if (isSuccessfulChangeRoleToAdmin.Message.IsSuccessful)
            {
                var newAnalyst = await _userRepository.GetUserByUserId(request.UserId);
                newAnalyst.RoleId = 4;
                newAnalyst.UpdatedAt = DateTime.UtcNow;
                await _filialAreaRepository.UpdateAnalystId(request.UserId, request.FilialId);
                await _unitOfWork.Commit(new CancellationToken());
                
                return new Response("The role is changed succesfully", 200);
            }
            else return new Response("Something wrong when tried to change role", 404);
        }
    }
}