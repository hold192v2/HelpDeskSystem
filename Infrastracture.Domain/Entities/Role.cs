using System.Text.Json.Serialization;

namespace Infrastracture.Domain.Entities;

public class Role
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}