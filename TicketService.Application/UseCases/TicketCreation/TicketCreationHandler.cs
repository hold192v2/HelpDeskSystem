
using MassTransit;
using MediatR;
using TicketService.Application.DTOs;
using TicketService.Application.Extentions;
using TicketService.Domain.Entities;
using TicketService.Domain.Interfaces;
using Response = TicketService.Application.HandlerResponse.Response;

namespace TicketService.Application.UseCases.TicketCreation;

public class TicketCreationHandler : IRequestHandler<TicketCreationRequest, Response>
{
    private readonly IRequestClient<CreationTicketCatalogRequestDto>  _catalogClient;
    private readonly IRequestClient<CreateTicketPerformersIdsGetRequestDto>  _performersClient;
    private readonly IPhotoConnectionRepository  _photoConnectionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketCommentRepository _ticketCommentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketCreationHandler(IRequestClient<CreationTicketCatalogRequestDto>  catalogClient, IPhotoConnectionRepository photoConnectionRepository,  IRequestClient<CreateTicketPerformersIdsGetRequestDto> performersClient,  ITicketRepository ticketRepository, ITicketCommentRepository ticketCommentRepository,  IUnitOfWork unitOfWork)
    {
        _catalogClient =  catalogClient;
        _photoConnectionRepository = photoConnectionRepository;
        _performersClient = performersClient;
        _ticketRepository = ticketRepository;
        _ticketCommentRepository = ticketCommentRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Response> Handle(TicketCreationRequest request, CancellationToken cancellationToken)
    {
        var slaResponse = await _catalogClient.GetResponse<CreationTicketCatalogDto>(
            new CreationTicketCatalogRequestDto(request.CategoryId, request.PriorityId));
        var slaHours = (int)Math.Round(slaResponse.Message.BaseCategorySlaPeriod * slaResponse.Message.PrioritySlaFactor);
        var creationDate = DateTimeOffset.UtcNow;
        var ticket = new Ticket()
        {
            Id = Guid.NewGuid(),
            Theme = request.Theme,
            ThemeSearch = request.Theme.Trim().ToLower(),
            Description = request.Description,
            Location = request.Location,
            CategoryId = request.CategoryId,
            OfficeId = request.OfficeId,
            PriorityId = request.PriorityId,
            CreateUserId = request.UserId,
            StatusId = (int)StatusEnum.Pending,
            AllocatedSeconds = slaHours * 3600,
            CreatedAt = creationDate,
        };
        
        await _ticketCommentRepository.CreateAsync(new TicketComment(
        )
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            UserId = Guid.Parse("236c775f-fc69-4e65-8265-7a48994febe4"),
            Message = "Заявка подана через веб-портал."
        });
        
        if (request.PhotosUrl.Any())
            await _photoConnectionRepository.CreateAsync(request.PhotosUrl.Select(url => new PhotoConnection()
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                PhotoUrl = url
            }).ToList());
        var performersResponse = await _performersClient.GetResponse<CreateTicketPerformersIdsGetDto>(
            new CreateTicketPerformersIdsGetRequestDto(request.OfficeId, request.CategoryId));
        var performersActiveTicketsCount =
            await _ticketRepository.PerformerTicketCount(
                performersResponse.Message.PerformersEvaluationInfo.Keys.ToList());
        if (performersActiveTicketsCount == null || performersActiveTicketsCount.Count == 0 
                                                 || performersResponse?.Message?.PerformersEvaluationInfo == null 
                                                 || performersResponse.Message.PerformersEvaluationInfo.Count == 0)
            return new Response("No performers found", 404);
        var activePerformer = performersActiveTicketsCount[performersActiveTicketsCount.Keys.Min()]
            .OrderByDescending(userId => performersResponse.Message.PerformersEvaluationInfo[userId])
            .First();
        ticket.PerformerId = activePerformer;
        ticket.StartedAt =  DateTimeOffset.UtcNow;
        ticket.DueAt = DateTimeOffset.UtcNow.AddHours(slaHours);
        ticket.StatusId =  (int)StatusEnum.Assigned;
        await _ticketCommentRepository.CreateAsync(new TicketComment(
        )
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            UserId = activePerformer,
            Message = "Назначен специалист."
        }); //при рефакторинге выкидывать в первую очередь.
        await _ticketRepository.Create(ticket);
        await _unitOfWork.Commit(new CancellationToken());
        return new Response("OK", 200);
        
    }
}