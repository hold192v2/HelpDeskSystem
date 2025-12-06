using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Yarp_API_Gateway.Controllers;
[ApiController]
[Route("gateway")]
public class GatewayController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    
    public GatewayController(IConfiguration config, IHttpClientFactory httpClientFactory,  IMemoryCache memoryCache)
    {
        _config = config;
        _httpClient = httpClientFactory.CreateClient("AllowAnyCert");
        _memoryCache = memoryCache;
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        var keycloakUrl = _config["Keycloak:AuthorizationUrl"];
        var clientId = _config["Keycloak:ClientId"];
        var redirectUri = _config["Keycloak:RedirectUri"];
        var codeVerifier = GenerateCodeVerifier();
        _memoryCache.Set("pkceCodeVerifier", codeVerifier);
        var authUrl = $"{keycloakUrl}?response_type=code" +
                      $"&client_id={clientId}" +
                      $"&scope=openid" +
                      $"&redirect_uri={redirectUri}" +
                      $"&code_challenge={GenerateCodeChallenge(codeVerifier)}" +
                      $"&code_challenge_method=S256";
        return Redirect(authUrl);
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetTokens([FromQuery] string code)
    {
        var codeVerifier = _memoryCache.Get<string>("pkceCodeVerifier");
        var tokenEndpoint = _config["Keycloak:TokenUrl"];
        var redirectUri = "https://localhost:7269/gateway/token";
        var form = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "client_id", _config["Keycloak:ClientId"]! },
            { "redirect_uri", redirectUri },
            { "scope", "openid profile"},
            {"client_secret", "9ttscO6oNh8ZUbYZXx5cUURkINcb3kIP" },
            {"code_verifier", codeVerifier! },
        };
        var response = await _httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(form));
        
        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            return BadRequest("Token exchange failed: " + err);
        }

        var tokenJson = await response.Content.ReadAsStringAsync();
        var tokens = JsonSerializer.Deserialize<TokenResponse>(tokenJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        return Ok(tokens);
    }
    
    public class TokenResponse
    {
        public string Access_token { get; set; }
        public string Refresh_token { get; set; }
        public string Id_token { get; set; }
        public int Expires_in { get; set; }
        public string Token_type { get; set; }
    }
    string GenerateCodeVerifier()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Base64UrlEncode(bytes);
    }

    string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        return Base64UrlEncode(challengeBytes);
    }

    string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}