using System.Text.Json.Serialization;

namespace Catalog.Domain.Entities;

public class Status
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("isFinal")]
    public bool IsFinal  { get; set; }
}