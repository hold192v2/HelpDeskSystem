namespace Infrastracture.Domain.Entities;

public class FilialArea: BaseEntity
{
    public int id { get; set; }
    public string name { get; set; }
    public Guid analiticId { get; set; }
}