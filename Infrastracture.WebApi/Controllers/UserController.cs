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
}