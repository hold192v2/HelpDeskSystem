using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Yarp_API_Gateway.DTOs;
using Yarp_API_Gateway.Extentions.ExtentionInterfaces;

namespace Yarp_API_Gateway.Extentions;

public class TokenRefresher : ICheckedExpiration, IRefreshToken, IExecuteStage, IUploadIntoMiddleware, IUploadContext
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    
    private HttpContext _httpContext = default!;
    private bool _isExpired = false;
    private SessionDTO _sessionDto = default!;
    //как должно работать 1. выдается httpContext 2. Смотрится время токена access 3.Если время просрочилось а.Обновляем токены через Keycloak и refresh б. добавляем в Middleware 4. Если нет, то отдаем нынешние токены 
    public TokenRefresher(IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
    }
    
    public static IUploadContext Start(TokenRefresher refresher)
        => refresher;
    
    public Task<ICheckedExpiration> UploadContextAsync(HttpContext httpContext)
    {
        _httpContext = httpContext;
        return Task.FromResult<ICheckedExpiration>(this);
    }

    public async Task<IRefreshToken> CheckExpirationAsync()
    {
        var expiresAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(await _httpContext.GetTokenAsync("expires_at"))).UtcDateTime;;
        _isExpired = DateTimeOffset.UtcNow > expiresAt;
        return this;
    }

    public async Task<IUploadIntoMiddleware> RefreshTokenAsyns()
    {
        if (!_isExpired)
            return this;
        _sessionDto = await RefreshTokensAsync();
        return this;
    }
    public async Task<IExecuteStage> UploadIntoMiddlewareAsync()
    {
        if (!_isExpired) return this;
        var authResult = await _httpContext.AuthenticateAsync();
        authResult.Properties.UpdateTokenValue("access_token", _sessionDto.AccessToken);
        authResult.Properties.UpdateTokenValue("refresh_token", _sessionDto.RefreshToken);
        authResult.Properties.UpdateTokenValue("id_token", _sessionDto.IdToken);
        authResult.Properties.UpdateTokenValue("expires_at", DateTimeOffset.UtcNow.AddSeconds(_sessionDto.ExpiresAt).ToUnixTimeSeconds().ToString());
        await _httpContext.SignInAsync(authResult.Principal, authResult.Properties);
        return this;
    }
    public async Task<SessionDTO> ExecuteAsync()
    {
        if (_isExpired)
            return _sessionDto;

        return await GetCurrentTokensAsync();
    }

    private async Task<SessionDTO?> GetCurrentTokensAsync()
    {
        var accessToken = await _httpContext.GetTokenAsync("access_token");
        var refreshToken = await _httpContext.GetTokenAsync("refresh_token");
        var expiresAt = (int)(DateTimeOffset.Parse(await _httpContext.GetTokenAsync("expires_at"))
                              - DateTimeOffset.UtcNow).TotalSeconds;
        return new SessionDTO(accessToken!, refreshToken!, expiresAt);
    }

    private async Task<SessionDTO> RefreshTokensAsync()
    {
        var refreshToken = await _httpContext.GetTokenAsync("refresh_token");
        var tokenEndpoint = _config["Keycloak:TokenUrl"];
        var form = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", _config["Keycloak:ClientId"]! },
            { "client_secret", _config["Keycloak:ClientSecret"]! },
            { "refresh_token", refreshToken! },
        };
        var httpClient = _httpClientFactory.CreateClient("AllowAnyCert");
        var response = await httpClient.PostAsync(
            tokenEndpoint,
            new FormUrlEncodedContent(form)
        );
        
        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new UnauthorizedAccessException("Refresh token failed: " + err);
        }
        return JsonSerializer.Deserialize<SessionDTO>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!; 
        
    }
    
}
