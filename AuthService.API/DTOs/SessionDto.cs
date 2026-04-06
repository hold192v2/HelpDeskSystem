using System.Text.Json.Serialization;

namespace DTOs;

public class SessionDTO
{
    public SessionDTO(string accessToken, string refreshToken, string idToken, long expiresAt)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        IdToken = idToken;
    }
    
    public string UserId { get; set; }
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; } 
    [JsonPropertyName("expires_in")]
    public long ExpiresAt { get; set; } 
    
    
}