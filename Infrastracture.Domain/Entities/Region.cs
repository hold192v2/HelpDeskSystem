namespace Infrastracture.Domain.Entities;

public class Region: BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int FilialId { get; set; }
    public Guid AdminId { get; set; }
}