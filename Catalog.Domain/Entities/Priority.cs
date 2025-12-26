using System.Text.Json.Serialization;

namespace Catalog.Domain.Entities;

public class Priority
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("sla-factor")]
    public double SlaFactor  { get; set; }
}