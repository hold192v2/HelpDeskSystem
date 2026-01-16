using Infrastracture.Application.DTOs;
using Infrastracture.Domain.Interfaces;
using MassTransit;
using MediatR;
using Response = Infrastracture.Application.HandlerResponse.Response;

namespace Infrastracture.Application.UseCases.Appointments.PerformerAppointment;

public class PerformerAppointmentHandler : IRequestHandler<PerformerAppointmentRequest, Response>
{
    private readonly IRequestClient<ChangeRoleRequestDto> _requestClient;
    private readonly IUserRepository _userRepository;
    private readonly IOfficeRepository _officeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryUserRepository _categoryUserRepository;

    public PerformerAppointmentHandler(IRequestClient<ChangeRoleRequestDto> requestClient,  IUserRepository userRepository,  IOfficeRepository officeRepository, IUnitOfWork unitOfWork, ICategoryUserRepository categoryUserRepository)
    {
        _requestClient = requestClient;
        _userRepository = userRepository;
        _officeRepository = officeRepository;
        _unitOfWork = unitOfWork;
        _categoryUserRepository =  categoryUserRepository;
    }
    public async Task<Response> Handle(PerformerAppointmentRequest request, CancellationToken cancellationToken)
    {
        var isSuccessfulChangeRole = await _requestClient.GetResponse<ChangeRoleResponse>(new ChangeRoleRequestDto()
        {
            RoleNameAssign = "performer",
            RolesNameDelete = new List<string>{"employee"},
            UserId = request.UserId
        });
        if (isSuccessfulChangeRole.Message.IsSuccessful)
        {
            var offices = await _officeRepository.GetOfficesByIdsAsync(request.OfficesIds);
            var user = await _userRepository.GetUserByUserId(request.UserId);
            user.RoleId = 2;
            await _categoryUserRepository.SaveRelationAsync(user.Id, request.CategoryIds);
            foreach (var item in offices)
            {
                if (user.Offices.All(x => x.Id != item.Id))
                    user.Offices.Add(item);
            }
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Commit(new CancellationToken());
            return new Response("The role is changed succesfully", 200);
        }
        else return new Response("Something wrong when tried to change role", 404);
    }
}