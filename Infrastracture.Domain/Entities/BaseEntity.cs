namespace Infrastracture.Domain.Entities;

public class BaseEntity
{
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }
    public DateTimeOffset DateDeleted { get; set; }
}