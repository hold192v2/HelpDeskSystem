using System.Text.Json.Serialization;

namespace Catalog.Domain.Entities;

public class Category
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("sla")]
    public int BasePeriodSla { get; set; }
}