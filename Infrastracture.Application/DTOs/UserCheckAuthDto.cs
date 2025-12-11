namespace DTOs;

public class UserCheckAuthDto
{
    public Guid? UserId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
    public string? RoleName { get; set; }
    public string? Avatar { get; set; }
}