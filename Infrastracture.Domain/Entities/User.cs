namespace Infrastracture.Domain.Entities;

public class User: BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Fullname { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public int RoleId { get; set; }
    public int CategoryId { get; set; }
    public Guid OfficeId { get; set; }
    public int RegionId { get; set; }
    public double Rating { get; set; }
    public string SystemId { get; set; }
}