using System.Text.Json.Serialization;

namespace Yarp_API_Gateway.DTOs;

public class SessionDTO
{
    public SessionDTO(string accessToken, string refreshToken, long expiresAt)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresAt = expiresAt;
        
    }
    
    public string UserId { get; set; }
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; } 
    [JsonPropertyName("expires_in")]
    public long ExpiresAt { get; set; } // Unix timestamp access token
    
    
}