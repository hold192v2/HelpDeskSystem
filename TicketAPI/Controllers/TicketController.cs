using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketService.Application.DTOs;
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
        var request = _mapper.Map<TicketPanelRequest>(requestDto, opt =>
        {
            opt.Items["roleName"] = mainRole;
            opt.Items["userId"] = userId;
        });
        
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("creation")]
    public async Task<IActionResult> CreateTicket()
    {
        return Ok();
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketInfo()
    {
        return Ok();
    }
    
    [Authorize]
    [HttpGet("comments")]
    public async Task<IActionResult> GetTicketComments()
    {
        return Ok();
    }
    
    
}