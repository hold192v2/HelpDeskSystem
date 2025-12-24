using System.Security.Claims;
using System.Text.Json;
using AuthService.API.Extentions;
using DTOs;
using Flurl.Http;
using Keycloak.Net;
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
    private readonly IRequestClient<UserCheckAuthRequestDto> _authClient;
    private readonly IRequestClient<RegisterIntoInfrastructureDto> _registerClient;
    private readonly IConfiguration _config;

    public AuthController(IRequestClient<UserCheckAuthRequestDto> authClient, IRequestClient<RegisterIntoInfrastructureDto> registerClient,  IConfiguration config)
    {
        _authClient = authClient;
        _registerClient = registerClient;
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
        var userResponse = await _authClient.GetResponse<UserCheckAuthDto>
            (new UserCheckAuthRequestDto { UserId = userId });

        if (userResponse.Message.UserId != null)
        {
            var createdUser = new AuthUserMeDto(userResponse.Message);
            return Ok(createdUser);
        }
        
        var tokenResponse = await "https://auth.alpha-helpdesk.ru/realms/HelpDeskKeycloak/protocol/openid-connect/token"
            .PostUrlEncodedAsync(new
            {
                grant_type = "client_credentials",
                client_id = "auth-adminAPI-client",
                client_secret = "rFX2eRscaWRsqLMA6mq3z1bCNqqXjOhJ"
            })
            .ReceiveJson<SessionDTO>();

        var kc = new KeycloakClient(
            "https://auth.alpha-helpdesk.ru",
            () => tokenResponse.AccessToken
        );

        var user = await kc.GetUserAsync("HelpDeskKeycloak", userId.ToString());

        var authNoUser = new AuthNoUserDto(user.LastName,
            user.FirstName,
            user.Attributes?["patronymic"]?.FirstOrDefault()!,
            user.Email, false);
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
        
        var tokenResponse = await "https://auth.alpha-helpdesk.ru/realms/HelpDeskKeycloak/protocol/openid-connect/token"
            .PostUrlEncodedAsync(new
            {
                grant_type = "client_credentials",
                client_id = "auth-adminAPI-client",
                client_secret = "rFX2eRscaWRsqLMA6mq3z1bCNqqXjOhJ"
            })
            .ReceiveJson<SessionDTO>();

        var kc = new KeycloakClient(
            "https://auth.alpha-helpdesk.ru",
            () => tokenResponse.AccessToken
        );

        var user = await kc.GetUserAsync("HelpDeskKeycloak", userId.ToString());
        
        var patronymic = user.Attributes["patronymic"].FirstOrDefault();

        var isSuccessfulRegister = await _registerClient.GetResponse<UserCheckAuthDto>
        (new RegisterIntoInfrastructureDto(userId, user.FirstName, user.LastName, patronymic,
            user.Email, 1, registerDto.OfficeId, registerDto.RegionId, await SystemIdGenerator.GetSystemIdAsync()));
        if (isSuccessfulRegister.Message.UserId != null)
        {
            return Ok(new AuthUserMeDto(isSuccessfulRegister.Message));
        }
        return NotFound();
    }
}