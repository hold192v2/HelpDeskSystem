using System.Text.Json.Serialization;

namespace Infrastracture.Domain.Entities;

public class FilialArea: BaseEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("analystId")]
    public Guid? AnaliticId { get; set; }
}