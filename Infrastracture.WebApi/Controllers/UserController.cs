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
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Infrastracture.WebApi.Controllers;
[ApiController]
[Route("user")]
public class UserController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUser _user;
    private readonly IRole _role;

    public UserController(IMediator mediator, IUser user, IRole role)
    {
        _mediator = mediator;
        _user = user;
        _role = role;
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
    public async Task<IActionResult> CreateNewOfficePost([FromBody] CreateNewOfficePostRequest request, [FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var trueRole = new List<string> { "admin", "superadmin" };
        if (!TrueRole(userId, trueRole))
            return BadRequest();
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    
    [HttpPatch("createNewOffice")]
    public async Task<IActionResult> CreateNewOfficePatch([FromBody] CreateNewOfficePatchRequest request, [FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var trueRole = new List<string> { "admin", "superadmin" };
        if (!TrueRole(userId, trueRole))
            return BadRequest();
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }

    [HttpGet("getPerformers")]
    public async Task<IActionResult> GetPerformers([FromQuery] PerformersRequest query, [FromHeader(Name = "X-User-Id")] Guid userId)
    {
        query = query with { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response.Performers);
    }

    [HttpGet("offices")]
    public async Task<IActionResult> GetOffices([FromQuery] OfficesRequest query, [FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var trueRole = new List<string> { "employee", "admin", "analyst","superadmin" };
        if (!TrueRole(userId, trueRole))
            return BadRequest();
        var response = await _mediator.Send(query);
        return Ok(response.Offices);
    }

    [HttpGet("searchUser")]
    public async Task<IActionResult> GetUsers([FromQuery] SearchUserRequest query, [FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var trueRole = new List<string> { "admin", "superadmin" };
        if (!TrueRole(userId, trueRole))
            return BadRequest();
        query = query with { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response.Users);
    }
    
    [HttpPost("adminAppointment")]
    public async Task<IActionResult> AdminAppointment([FromBody] AdminAppointmentRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    
    [HttpPost("analiticAppointment")]
    public async Task<IActionResult> AnaliticAppointment([FromBody] AnaliticAppointmentRequest request)
    {
        var response = await _mediator.Send(request);
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

    private bool TrueRole(Guid userId, List<string> trueRoles)
    {
        var user = _user.GetUserByUserId(userId).Result;
        var role = _role.GetRoleByUser(user).Result;
        return trueRoles.Contains(role.Name);
    }
}