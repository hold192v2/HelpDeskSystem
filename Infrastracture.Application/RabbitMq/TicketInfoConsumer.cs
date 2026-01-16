using Infrastracture.Domain.Interfaces;
using MassTransit;
using TicketService.Application.DTOs;

namespace DTOs;

public class TicketInfoConsumer : IConsumer<TicketIntoInfrastructureRequestDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IOfficeRepository _officeRepository;

    public TicketInfoConsumer(IUserRepository userRepository, IOfficeRepository officeRepository)
    {
        _userRepository = userRepository;
        _officeRepository = officeRepository;
    }
    public async Task Consume(ConsumeContext<TicketIntoInfrastructureRequestDto> context)
    {
        var username = await _userRepository.GetUserName(context.Message.PerformerId);
        var officeName = await _officeRepository.GetOfficeNameByIdAsync(context.Message.OfficeId);
        await context.RespondAsync(new TicketInfoInfrastructureDto(officeName, username));
    }
}