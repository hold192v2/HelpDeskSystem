using System.Text.Json.Serialization;

namespace Infrastracture.Domain.Entities;

public class Region: BaseEntity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("filialId")]
    public int FilialId { get; set; }
    [JsonPropertyName("adminId")]
    public Guid? AdminId { get; set; }
}