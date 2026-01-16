using Infrastracture.Domain.Interfaces;
using MassTransit;
using TicketService.Application.DTOs;

namespace DTOs;

public class NameOfficesConsumer : IConsumer<OfficeNameGetRequestDto>
{
    private readonly IOfficeRepository _officeRepository;

    public NameOfficesConsumer(IRegionRepository regionRepository, IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }
    
    public async Task Consume(ConsumeContext<OfficeNameGetRequestDto> context)
    {
        var response = await _officeRepository.GetOfficesNamesByIdAsync((List<Guid>)context.Message.OfficeIds);
        await context.RespondAsync(new OfficeNameGetDto(response));
    }
}