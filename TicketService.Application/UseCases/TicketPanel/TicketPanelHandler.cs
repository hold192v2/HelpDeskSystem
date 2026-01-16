using System.ComponentModel;
using MassTransit;
using MediatR;
using TicketService.Application.DTOs;
using TicketService.Application.Extentions.Interfaces;
using TicketService.Application.Extentions.TicketUsability;
using TicketService.Domain.Interfaces;
using Response = TicketService.Application.HandlerResponse.Response;

namespace TicketService.Application.UseCases.TicketPanel;

public class TicketPanelHandler : IRequestHandler<TicketPanelRequest, Response>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IRequestClient<AdminOfficesGetRequestDto> _adminOfficesClient;
    private readonly IRequestClient<OfficeNameGetRequestDto> _nameOfficesClient;
    private readonly IRequestClient<CatalogTicketPanelRequestDto> _catalogClient;
    public TicketPanelHandler(ITicketRepository ticketRepository, IRequestClient<AdminOfficesGetRequestDto> adminOfficesClient,  IRequestClient<OfficeNameGetRequestDto> nameOfficesClient,  IRequestClient<CatalogTicketPanelRequestDto> catalogClient)
    {
        _ticketRepository = ticketRepository;
        _adminOfficesClient = adminOfficesClient;
        _nameOfficesClient = nameOfficesClient;
        _catalogClient =  catalogClient;
    }
    public async Task<Response> Handle(TicketPanelRequest request, CancellationToken cancellationToken)
    {
        var searchString = request.Theme!.ToLower().Trim();
        AdminOfficesGetDto offices = request.Role == "admin"
            ? (await _adminOfficesClient.GetResponse<AdminOfficesGetDto>(
                new AdminOfficesGetRequestDto(request.UserId))).Message
            : new AdminOfficesGetDto(new List<Guid>());
        
        var specification = Resolve(request.Role,  offices.OfficeIds);
        var baseQuery = _ticketRepository.Query();
        baseQuery = specification.Apply(baseQuery, new TicketPanelUsabilityDto
            (
                request.Page, 
                request.PriorityId, 
                request.StatusId, 
                request.SortByDate, 
                request.Theme, 
                request.Role, 
                request.UserId, 
                offices.OfficeIds
            ));
        
        var tickets = await _ticketRepository.GetTicketsForPanelWithSearch(baseQuery, request.Page, 10, request.PriorityId, request.StatusId, (int)request.SortByDate, searchString);
        var officeNamesDictionary = await _nameOfficesClient.GetResponse<OfficeNameGetDto>(
            new OfficeNameGetRequestDto(tickets
                .Select(x => x.OfficeId)
                .Distinct()
                .ToList()));
        var catalogNameResponse = await _catalogClient.GetResponse<CatalogTicketPanelDto>(
            new CatalogTicketPanelRequestDto(tickets.Select(ticket => ticket.StatusId)
                    .Distinct()
                    .ToList(),
                tickets.Select(ticket => ticket.PriorityId)
                    .Distinct()
                    .ToList()
            )
        );
        
        var ticketCount = await _ticketRepository.CountAsync(baseQuery, request.PriorityId, request.StatusId, (int)request.SortByDate, searchString);
        var paginationResponse = new PaginationModel(request.Page, ticketCount, (ticketCount + 9)/10);
        var contentResponse = tickets.Select(ticket =>
            {
                var officeName = officeNamesDictionary.Message.OfficeIdNames
                    .FirstOrDefault(x => x.OfficeId == ticket.OfficeId)!.OfficeName;
                return new ContentTicketModel(
                    ticket.Id,
                    ticket.Number,
                    ticket.Theme,
                    ticket.Description,
                    officeName,
                    catalogNameResponse.Message.PriorityNames[ticket.PriorityId - 1],
                    catalogNameResponse.Message.StatusNames[ticket.StatusId - 1],
                    ticket.DueAt,
                    ticket.DueAt < DateTimeOffset.Now
                );
            }
        );
        return new Response("OK", 200, new GetTicketPanelDto(contentResponse.ToList(), paginationResponse));
    }
    private ITicketRoleVisibilitySpecification Resolve(string role, List<Guid> officeIds)
    {
        return role switch
        {
            "employee" => new EmployeeTicketVisibilitySpec(),
            "performer" => new PerformerTicketVisibilitySpec(),
            "admin" => new AdminTicketVisibilitySpec(officeIds),
            _ => throw new InvalidEnumArgumentException()
        };
    }
}