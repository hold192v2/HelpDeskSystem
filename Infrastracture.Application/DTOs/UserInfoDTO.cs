namespace Infrastracture.Application.DTOs;

public class UserInfoDTO
{
    public string name { get; set; }
    public string surname { get; set; }
    public string patrinymic { get; set; }
    public string rolename { get; set; }
    public string email { get; set; }
    public List<string> category { get; set; }
    public double rating { get; set; }
    public List<string> office { get; set; }
    public int regionId { get; set; }
    public string systemId { get; set; }
    public string avatar { get; set; }
}