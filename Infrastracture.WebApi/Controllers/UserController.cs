using System.Security.Claims;
using Infrastracture.Application.DTOs;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infrastracture.WebApi.Controllers;
[ApiController]
[Microsoft.AspNetCore.Mvc.Route("user")]
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
    [Authorize] //ввести адекватную авторизацию, все ломается
    [HttpGet("getUserInfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var request = new UserInfoRequest(userId);
        var response = await _mediator.Send(request);
        return Ok(response.UserInfo);
    }
    [Authorize]
    [HttpGet("getUserPanel")]
    public async Task<IActionResult> GetUserPanel()
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        
        var request = new UserPanelRequest(userId, true);
        var response = await _mediator.Send(request);
        return Ok(response.UserPanel);
    }
    [Authorize]
    [HttpPost("createNewOffice")]
    public async Task<IActionResult> CreateNewOfficePost([FromBody] CreateNewOfficeQueryDto request)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        
        var response = await _mediator.Send(new CreateNewOfficePostRequest(request.City, request.Address, request.RegionId, userId));
        if (response is null)
            return BadRequest();
        return Ok();
    }
    [Authorize]
    [HttpPatch("createNewOffice")]
    public async Task<IActionResult> CreateNewOfficePatch([FromBody] CreateNewOfficePatchRequest request)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        
        var trueRole = new List<string> { "admin", "superadmin" };
        if (!TrueRole(userId, trueRole))
            return BadRequest();
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    [Authorize]
    [HttpGet("getPerformers")]
    public async Task<IActionResult> GetPerformers([FromQuery] PerformersRequest query)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        
        query = query with { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response.Performers);
    }
    [Authorize]
    [HttpGet("offices")]
    public async Task<IActionResult> GetOffices([FromQuery] OfficeQueryDto query)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var request = new OfficesRequest(query.RegionId, query.FillialId, userId);
        
        var response = await _mediator.Send(request);
        return Ok(response.Offices);
    }
    [Authorize]
    [HttpGet("searchUser")]
    public async Task<IActionResult> GetUsers([FromQuery] SearchUserRequest query)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        
        var trueRole = new List<string> { "admin", "superadmin" };
        if (!TrueRole(userId, trueRole))
            return BadRequest();
        query = query with { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response.Users);
    }
    [Authorize]
    [HttpPost("adminAppointment")]
    public async Task<IActionResult> AdminAppointment([FromBody] AdminAppointmentRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    [Authorize]
    [HttpPost("analiticAppointment")]
    public async Task<IActionResult> AnaliticAppointment([FromBody] AnaliticAppointmentRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    [Authorize]
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