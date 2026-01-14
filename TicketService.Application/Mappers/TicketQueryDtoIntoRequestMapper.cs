using AutoMapper;
using TicketService.Application.DTOs;
using TicketService.Application.UseCases.TicketPanel;

namespace TicketService.Application.Mappers;

public class TicketQueryDtoIntoRequestMapper : Profile
{
    public TicketQueryDtoIntoRequestMapper()
    {
        CreateMap<TicketPanelQueryDto, TicketPanelRequest>()
            .ForMember(request => request.Role, opt => opt.MapFrom((src, dto, _, context) => context.Items["roleName"]))
            .ForMember(request => request.UserId, opt  => opt.MapFrom((src, dto, _, context) => context.Items["userId"]));
    }
}