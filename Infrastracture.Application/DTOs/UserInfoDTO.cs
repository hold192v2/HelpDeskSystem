namespace Infrastracture.Application.DTOs;

public class UserInfoDTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string RoleName { get; set; }
    public string Email { get; set; }
    public List<string> Category { get; set; }
    public double? Rating { get; set; }
    public List<string> Office { get; set; }
    public string Region { get; set; }
    public string SystemId { get; set; }
    public string Avatar { get; set; }
}