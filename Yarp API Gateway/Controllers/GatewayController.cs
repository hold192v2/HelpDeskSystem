using Microsoft.AspNetCore.Mvc;

namespace Yarp_API_Gateway.Controllers;
[ApiController]
[Route("gateway")]
public class GatewayController : ControllerBase
{
    private readonly IConfiguration _config;

    public GatewayController(IConfiguration config)
    {
        _config = config;
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        var keycloakUrl = _config["Keycloak:AuthorizationUrl"];
        var clientId = "apiGateway-helpdesk";
        var redirectUri = _config["Keycloak:RedirectUri"];
        var state = Guid.NewGuid().ToString();
        var authUrl = $"http://localhost:8080/realms/HelpDeskKeycloak/protocol/openid-connect/auth?response_type=code&client_id=apiGateway-helpdesk&redirect_uri=https%3A%2F%2Flocalhost%3A5299%2Fswagger%2Foauth2-redirect.html&state=V2VkIERlYyAwMyAyMDI1IDEyOjA0OjAxIEdNVCswNTAwICjQldC60LDRgtC10YDQuNC90LHRg9GA0LMsINGB0YLQsNC90LTQsNGA0YLQvdC%2B0LUg0LLRgNC10LzRjyk%3D&code_challenge=AUqE-fYse_JyhOiOEcU2Zxhq2yr3uu8D588kUGnDdn4&code_challenge_method=S256";
        return Redirect(authUrl);
    }
}