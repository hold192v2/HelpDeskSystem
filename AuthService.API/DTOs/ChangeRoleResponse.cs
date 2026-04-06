namespace Infrastracture.Application.DTOs;

public class ChangeRoleResponse
{
    public bool IsSuccessful { get; set; }

    public ChangeRoleResponse(bool isSuccessful)
    {
        this.IsSuccessful = isSuccessful;
    }
}