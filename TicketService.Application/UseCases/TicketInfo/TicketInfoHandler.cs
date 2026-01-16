using MassTransit;
using MediatR;
using TicketService.Application.DTOs;
using TicketService.Domain.Interfaces;
using Response = TicketService.Application.HandlerResponse.Response;

namespace TicketService.Application.UseCases.TicketInfo;

public class TicketInfoHandler : IRequestHandler<TicketInfoRequest, Response>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IPhotoConnectionRepository _photoConnectionRepository;
    private readonly IRequestClient<TicketIntoInfrastructureRequestDto> _infrastructureClient;
    private readonly IRequestClient<TicketInfoCatalogRequestDto> _catalogClient;

    public TicketInfoHandler(ITicketRepository ticketRepository, IPhotoConnectionRepository photoConnectionRepository,  IRequestClient<TicketIntoInfrastructureRequestDto> infrastructureClient, IRequestClient<TicketInfoCatalogRequestDto> catalogClient)
    {
        _ticketRepository = ticketRepository;
        _photoConnectionRepository = photoConnectionRepository;
        _infrastructureClient = infrastructureClient;
        _catalogClient = catalogClient;
    }
    public async Task<Response> Handle(TicketInfoRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetTicketById(request.TicketId);
        var responseDto = new TicketInfoDto(ticket);
        var photos = await _photoConnectionRepository.GetByTicketIdAsync(ticket.Id);
        var infrastructureResponse =
            await _infrastructureClient.GetResponse<TicketInfoInfrastructureDto>(
                new TicketIntoInfrastructureRequestDto(ticket.OfficeId, ticket.PerformerId));
        var catalogResponse =
            await _catalogClient.GetResponse<TicketInfoCatalogDto>(
                new TicketInfoCatalogRequestDto(ticket.StatusId, ticket.PriorityId, ticket.CategoryId));
        responseDto.Photos = photos.Select(x => x.PhotoUrl).ToList();
        responseDto.Office = infrastructureResponse.Message.OfficeName;
        responseDto.PerformerName = infrastructureResponse.Message.PerformerName;
        responseDto.CategoryName = catalogResponse.Message.CategoryName;
        responseDto.Status = catalogResponse.Message.StatusName;
        responseDto.Priority = catalogResponse.Message.PriorityName;
        return new Response("Ok", 200,  responseDto);
    }
}