using Infrastracture.Domain.Interfaces;
using MassTransit;
using TicketService.Application.DTOs;

namespace DTOs;

public class AdminOfficesGetConsumer : IConsumer<AdminOfficesGetRequestDto>
{
    private readonly IRegionRepository  _regionRepository;
    private readonly IOfficeRepository _officeRepository;

    public AdminOfficesGetConsumer(IRegionRepository regionRepository, IOfficeRepository officeRepository)
    {
        _regionRepository = regionRepository;
        _officeRepository = officeRepository;
    }
    
    public async Task Consume(ConsumeContext<AdminOfficesGetRequestDto> context)
    {
        var regionId = await _regionRepository.GetRegionIdByUserId(context.Message.AdminId);
        var officesIds = await _officeRepository.GetOfficesIdsByRegionIdAsync(regionId);
        await context.RespondAsync(new AdminOfficesGetDto(officesIds));
    }
}