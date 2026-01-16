using AutoMapper;
using MassTransit.SagaStateMachine;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketService.Application.DTOs;
using TicketService.Application.UseCases.TicketCreation;
using TicketService.Application.UseCases.TicketInfo;
using TicketService.Application.UseCases.TicketPanel;

namespace TicketAPI.Controllers;

[ApiController]
[Route("ticket")]
public class TicketController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TicketController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    [Authorize]
    [HttpGet("panel")]
    public async Task<IActionResult> GetTicketsForPanel([FromQuery] TicketPanelQueryDto requestDto)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var roles = JsonConvert.DeserializeObject<RoleContainer>(claims.GetValueOrDefault("realm_access")).Roles;
        var mainRole = roles.FirstOrDefault(r => r == "admin" || r == "employee" || r == "performer");
        var request = new TicketPanelRequest(requestDto.Page, 
            requestDto.PriorityId, 
            requestDto.StatusId, 
            requestDto.SortByDate, 
            requestDto.Theme, 
            mainRole!, 
            userId); 
        
        var response = await _mediator.Send(request);
        return Ok(response.GetTicketPanelDto);
    }

    [Authorize(Roles = "employee")]
    [HttpPost("creation")]
    public async Task<IActionResult> CreateTicket([FromBody] CreateQueryDto requestBody)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var request = new TicketCreationRequest(requestBody, userId);
        var response = await _mediator.Send(request);
        if (response.Status != 200)
            BadRequest(response.Message);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTicketInfo(Guid id)
    {
        var request = new TicketInfoRequest(id);
        var response = await _mediator.Send(request);
        return Ok(response.TicketInfoDto);
    }
    
    [Authorize]
    [HttpGet("comments")]
    public async Task<IActionResult> GetTicketComments()
    {
        return Ok();
    }
    
    
}