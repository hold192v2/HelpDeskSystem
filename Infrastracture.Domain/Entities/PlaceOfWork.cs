namespace Infrastracture.Domain.Entities;

public class PlaceOfWork: BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OfficeId { get; set; }
}