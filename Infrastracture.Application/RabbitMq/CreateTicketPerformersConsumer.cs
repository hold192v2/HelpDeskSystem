using Infrastracture.Domain.Interfaces;
using MassTransit;
using TicketService.Application.DTOs;

namespace DTOs;

public class CreateTicketPerformersConsumer : IConsumer<CreateTicketPerformersIdsGetRequestDto>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IUserRepository _userRepository;

    public CreateTicketPerformersConsumer(IOfficeRepository officeRepository, IRegionRepository regionRepository,  IUserRepository userRepository)
    {
        _officeRepository = officeRepository;
        _regionRepository = regionRepository;
        _userRepository = userRepository;
    }
    public async Task Consume(ConsumeContext<CreateTicketPerformersIdsGetRequestDto> context)
    {
        var performers = await _userRepository.GetPerformersByOfficeId(context.Message.OfficeId, context.Message.CategoryId);
        if (performers.Count == 0)
        {
            var office = await _officeRepository.GetOfficeByIdAsync(context.Message.OfficeId);
            performers = await _userRepository.GetPerformersWithSearchByRegionId(office.RegionId, 1, 50, "",
                [context.Message.CategoryId]);
        }
        await context.RespondAsync(new CreateTicketPerformersIdsGetDto(performers.Select(performer => new
        {
            Id =  performer.Id,
            Rating =  performer.Rating,
        }).ToDictionary(perform => perform.Id, perform => perform.Rating)));

    }
}