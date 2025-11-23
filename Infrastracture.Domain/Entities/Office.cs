namespace Infrastracture.Domain.Entities;

public class Office: BaseEntity
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public int RegionId { get; set; }
}