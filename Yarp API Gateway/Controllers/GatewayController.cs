using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Yarp_API_Gateway.DTOs;
using Yarp_API_Gateway.Extentions;

namespace Yarp_API_Gateway.Controllers;
[ApiController]
[Route("gateway")]
public class GatewayController : ControllerBase
{
    private TokenRefresher _tokenRefresher;
    public GatewayController(IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _tokenRefresher = new TokenRefresher(config, httpClientFactory);
    }
    

    [Authorize(AuthenticationSchemes = "OpenIdConnect")]
    [HttpGet("try")]
    public async Task<IActionResult> GetToken()
    {
        var tokens = await (await (await (await (await 
                 TokenRefresher
                     .Start(_tokenRefresher)
                     .UploadContextAsync(HttpContext))
                     .CheckExpirationAsync())
                     .RefreshTokenAsyns())
                     .UploadIntoMiddlewareAsync())
                     .ExecuteAsync();
        return Ok(tokens);
    }
    
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            return Redirect(returnUrl ?? "https://service-desk.website.yandexcloud.net");
        }
        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? "https://service-desk.website.yandexcloud.net"
            },
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }
    [Authorize]
    [HttpGet("logout")]
    public IActionResult Logout([FromQuery] string? returnUrl)
    {
        return SignOut(
            new AuthenticationProperties
            {
                RedirectUri = "https://socially-advantaged-moth.cloudpub.ru/gateway/login"
            },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }
    
}