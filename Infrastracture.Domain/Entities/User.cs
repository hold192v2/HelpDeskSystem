namespace Infrastracture.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; } = "";
    public int RoleId { get; set; }
    public int? CategoryId { get; set; }
    public ICollection<Office> Offices { get; set; } = new List<Office>();
    public int RegionId { get; set; }
    public double? Rating { get; set; }
    public string SystemId { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}