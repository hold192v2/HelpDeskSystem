using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API;
[ApiController]
[Route("users")]
public class AuthController : ControllerBase
{
    private static bool isExisting = true;

    [Authorize]
    [HttpGet("me")]
    public IActionResult AuthCheck()
    {
        if (isExisting)
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
    [Authorize(Policy = "EmployeePolicy")]
    [HttpPost("createUser")]
    public IActionResult CreateUser()
    {
        
        return StatusCode(203, "Пользователь не создан, необходимо указать офис");
            
    }

    [Authorize(Policy = "EmployeePolicy")]
    [HttpGet("employee")]
    public IActionResult AuthEmployeeTest()
    {
        return Ok(User.Claims.ToDictionary(c => c.Type, c => c.Value).GetValueOrDefault("name"));
    }
    [Authorize(Policy = "PerformerPolicy")]
    [HttpGet("performer")]
    public IActionResult AuthPerformerTest()
    {
        return Ok(User.Claims.ToDictionary(c => c.Type, c => c.Value).GetValueOrDefault("name"));
    }
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("admin")]
    public IActionResult AuthAdminTest()
    {
        return Ok(User.Claims.ToDictionary(c => c.Type, c => c.Value).GetValueOrDefault("name"));
    }
    [Authorize(Policy = "AnalystPolicy")]
    [HttpGet("analyst")]
    public IActionResult AuthAnalystTest()
    {
        return Ok(User.Claims.ToDictionary(c => c.Type, c => c.Value).GetValueOrDefault("name"));
    }
    [Authorize(Policy = "SuperadminPolicy")]
    [HttpGet("superadmin")]
    public IActionResult AuthSuperAdminTest()
    {
        return Ok(User.Claims.ToDictionary(c => c.Type, c => c.Value).GetValueOrDefault("name"));
    }
}