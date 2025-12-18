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
        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.First().Value);
        var userId = new Guid(claims.GetValueOrDefault("user-id")!);
        var userResponse = await _client.GetResponse<UserCheckAuthDto>
            (new UserCheckAuthRequestDto { UserId = userId });

        if (userResponse.Message.UserId != null)
        {
            var createdUser = new AuthUserMeDto(userResponse.Message);
            return Ok(createdUser);
        }

        var keycloak = new Keycloak.Net.KeycloakClient(
            _config["Authentication:ValidUrl"]!,
            _config["Keycloak:ClientSecret"]!
        );
        
        var user = await keycloak.GetUserAsync("HelpDeskKeycloak", userId.ToString());
        var authNoUser = new AuthNoUserDto(user.Attributes?["surname"]?.FirstOrDefault()!,
            user.Attributes?["name"]?.FirstOrDefault()!,
            user.Attributes?["patronymic"]?.FirstOrDefault()!,
            user.Attributes?["email"]?.FirstOrDefault()!, false);
        return Ok(authNoUser);

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
}