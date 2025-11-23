using Infrastracture.Application.UseCases.AdminAppointment;
using Infrastracture.Application.UseCases.AnaliticAppointment;
using Infrastracture.Application.UseCases.CreateNewOffice.Patch;
using Infrastracture.Application.UseCases.CreateNewOffice.Post;
using Infrastracture.Application.UseCases.Offices;
using Infrastracture.Application.UseCases.Performers;
using Infrastracture.Application.UseCases.Regions;
using Infrastracture.Application.UseCases.SearchUser;
using Infrastracture.Application.UseCases.UserInfo;
using Infrastracture.Application.UseCases.UserPanel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Infrastracture.WebApi.Controllers;
[ApiController]
[Route("user")]
public class UserController: ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getUserInfo")]
    public async Task<IActionResult> GetUserInfo([FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var request = new UserInfoRequest(userId);
        var response = await _mediator.Send(request);
        return Ok(response.UserInfo);
    }

    [HttpGet("getUserPanel")]
    public async Task<IActionResult> GetUserPanel([FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var request = new UserPanelRequest(userId, true);
        var response = await _mediator.Send(request);
        return Ok(response.UserPanel);
    }

    [HttpPost("createNewOffice")]
    public async Task<IActionResult> CreateNewOfficePost([FromBody] CreateNewOfficePostRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    
    [HttpPatch("createNewOffice")]
    public async Task<IActionResult> CreateNewOfficePatch([FromBody] CreateNewOfficePatchRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response is null)
            return BadRequest();
        return Ok();
    }

    [HttpGet("getPerformers")]
    public async Task<IActionResult> GetPerformers([FromQuery] PerformersRequest query, 
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response.Performers);
    }

    [HttpGet("offices")]
    public async Task<IActionResult> GetOffices([FromQuery] OfficesRequest query, 
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response.Offices);
    }

    [HttpGet("searchUser")]
    public async Task<IActionResult> GetUsers([FromQuery] SearchUserRequest query, 
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response.Users);
    }
    
    [HttpPost("adminAppointment")]
    public async Task<IActionResult> AdminAppointment([FromBody] AdminAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    
    [HttpPost("analiticAppointment")]
    public async Task<IActionResult> AnaliticAppointment([FromBody] AnaliticAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    
    [HttpGet("regions")]
    public async Task<IActionResult> Regions()
    {
        var request = new RegionsRequest();
        var response = await _mediator.Send(request);
        return Ok(response.Regions);
    }

}