namespace Infrastracture.Domain.Entities;

public class User: BaseEntity
{
    public Guid userId { get; set; }
    public string fullname { get; set; }
}