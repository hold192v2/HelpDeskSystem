namespace Infrastracture.Application.DTOs;

public class UserInfoDTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patrinymic { get; set; }
    public string Rolename { get; set; }
    public string Email { get; set; }
    public List<string> Category { get; set; }
    public double Rating { get; set; }
    public List<string> Office { get; set; }
    public int RegionId { get; set; }
    public string SystemId { get; set; }
    public string Avatar { get; set; }
}