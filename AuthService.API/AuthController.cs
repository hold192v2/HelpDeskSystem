using System.Security.Claims;
using System.Text.Json;
using AuthService.API.Extentions;
using DTOs;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IRequestClient<UserCheckAuthRequestDto> _client;
    private readonly IConfiguration _config;

    public AuthController(IRequestClient<UserCheckAuthRequestDto> client, IConfiguration config)
    {
        _client = client;
        _config = config;
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult> AuthCheck()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized(); 
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var isExisting = await _client.GetResponse<UserCheckAuthDto>
            (new UserCheckAuthRequestDto { UserId = userId });
        if (isExisting.Message.UserId != null)
        {
            return Ok(isExisting.Message);
        }
        var keycloak = new Keycloak.Net.KeycloakClient(
            _config["Authentication:ValidIssuer"]!,
            _config["Keycloak:ClientSecret"]!
        );
        return NoContent();

    }

    [Authorize]
    [HttpPost("registration")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var keycloak = new Keycloak.Net.KeycloakClient(
            _config["Authentication:ValidIssuer"]!,
            _config["Keycloak:ClientSecret"]!
        );
        var user = await keycloak.GetUserAsync("apiGateway-helpdesk", userId.ToString());
        var patronymic = user.Attributes?["patronymic"]?.FirstOrDefault();
        var email = user.Attributes?["email"]?.FirstOrDefault();
        var isSuccessfulRegister = await _client.GetResponse<UserCheckAuthDto>
        (new RegisterIntoInfrastructureDto(userId, user.FirstName, user.LastName, patronymic,
            email, 1, registerDto.OfficeId, registerDto.RegionId, await SystemIdGenerator.GetSystemIdAsync()));
        if (isSuccessfulRegister.Message.UserId != null)
        {
            return Ok(isSuccessfulRegister.Message);
        }
        return NotFound();
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