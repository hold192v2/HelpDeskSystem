namespace Infrastracture.Domain.Entities;

public class Office: BaseEntity
{
    public string city { get; set; }
    public string address { get; set; }
    public string regionId { get; set; }
}