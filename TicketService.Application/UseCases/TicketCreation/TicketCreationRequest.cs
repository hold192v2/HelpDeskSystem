using MediatR;
using TicketService.Application.DTOs;
using TicketService.Application.HandlerResponse;

namespace TicketService.Application.UseCases.TicketCreation;

public class TicketCreationRequest : IRequest<Response>
{
    public string Theme { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> PhotosUrl { get; set; }
    public Guid OfficeId { get; set; }
    public int PriorityId { get; set; }
    public Guid CategoryId { get; set; }
    public string Location { get; set; } = "";
    public Guid UserId { get; set; }

    public TicketCreationRequest(CreateQueryDto queryDto, Guid userId)
    {
        Theme = queryDto.Theme;
        Description = queryDto.Description;
        PhotosUrl = queryDto.PhotosUrl;
        OfficeId = queryDto.OfficeId;
        PriorityId = queryDto.PriorityId;
        CategoryId = queryDto.CategoryId;
        Location = queryDto.Location;
        UserId = userId;
    }
}