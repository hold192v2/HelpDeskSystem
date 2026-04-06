namespace Infrastracture.Application.DTOs;

public class ChangeRoleRequestDto
{
    public Guid UserId { get; set; }
    public List<string> RolesNameDelete { get; set; }
    public string RoleNameAssign { get; set; } = "";
}