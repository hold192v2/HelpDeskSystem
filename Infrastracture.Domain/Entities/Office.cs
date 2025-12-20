using System.Text.Json.Serialization;

namespace Infrastracture.Domain.Entities;

public class Office
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("city")]
    public string City { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
    [JsonPropertyName("regionId")]
    public int RegionId { get; set; }
    
    public ICollection<User> Users { get; set; } = new List<User>();
}