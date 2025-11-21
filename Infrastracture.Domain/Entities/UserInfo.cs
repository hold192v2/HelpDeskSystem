namespace Infrastracture.Domain.Entities;

public class UserInfo: BaseEntity
{
    public string name { get; set; }
    public string surname { get; set; }
    public string patronymic { get; set; }
    public string rolename { get; set; }
    public string email { get; set; }
    public List<string> category { get; set; }
    public double rating { get; set; }
    public List<string> office { get; set; }
    public int regionId { get; set; }
    public string systemId { get; set; }
    public string avatar { get; set; }
}