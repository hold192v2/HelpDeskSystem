namespace Infrastracture.Domain.Entities;

public class User: BaseEntity
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public string patronymic { get; set; }
    public string email { get; set; }
    public string avatar { get; set; }
    public int roleId { get; set; }
    public int categoryId { get; set; }
    public Guid officeId { get; set; }
    public int regionId { get; set; }
    public double rating { get; set; }
    public string systemId { get; set; }
}