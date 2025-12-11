using System.Security.Claims;
using System.Text.Json;
using DTOs;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API;
[ApiController]
[Route("users")]
public class AuthController : ControllerBase
{
    private readonly IRequestClient<UserCheckAuthRequestDto> _client;

    public AuthController(IRequestClient<UserCheckAuthRequestDto> client)
    {
        _client = client;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult> AuthCheck()
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var isExisting = await _client.GetResponse<UserCheckAuthDto>
        (new UserCheckAuthRequestDto {UserId = userId});
        if (isExisting.Message.UserId != null)
        {
            return Ok(isExisting.Message);
        }
        return StatusCode(203, "Пользователь не создан, необходимо указать офис");
            
    }
    
    [Authorize]
    [HttpGet("register")]
    public async Task<ActionResult> Register()
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var isExisting = await _client.GetResponse<UserCheckAuthDto>
            (new UserCheckAuthRequestDto {UserId = userId});
        if (isExisting.Message.UserId != null)
        {
            return Ok(isExisting.Message);
        }
        return StatusCode(203, "Пользователь не создан, необходимо указать офис");
            
    }
    
    [Authorize]
    [HttpGet("me2")]
    public IActionResult AuthCheck2()
    {
        if (true)
        {
            var realmAccessClaim = User.Claims.FirstOrDefault(c => c.Type == "realm_access")?.Value;
            var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
            var name = claims.GetValueOrDefault("name");
            var realmAccess = JsonDocument.Parse(realmAccessClaim);
            var roles = realmAccess.RootElement
                .GetProperty("roles")
                .EnumerateArray()
                .Select(r => r.GetString())
                .ToList();
            return Ok(new
            {
                Name = name,
                Roles = roles
            });
        }
        return StatusCode(203, "Пользователь не создан, необходимо указать офис");
            
    }
    
}