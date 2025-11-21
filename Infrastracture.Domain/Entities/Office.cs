namespace Infrastracture.Domain.Entities;

public class Office: BaseEntity
{
    public Guid id { get; set; }
    public string city { get; set; }
    public string address { get; set; }
    public int regionId { get; set; }
}