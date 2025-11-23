namespace Infrastracture.Domain.Entities;

public class FilialArea: BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Guid AnaliticId { get; set; }
}