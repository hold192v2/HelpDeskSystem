namespace Infrastracture.Domain.Entities;

public class CategoryUser
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
}