namespace DTOs;

public class AuthUserMeDto : UserCheckAuthDto
{
    public AuthUserMeDto(UserCheckAuthDto userResonse)
    {
        this.UserId = userResonse.UserId;
        this.Surname = userResonse.Surname;
        this.Name = userResonse.Name;
        this.Patronymic = userResonse.Patronymic;
        this.RoleName = userResonse.RoleName;
        this.Avatar =  userResonse.Avatar;
        this.IsExist = true;
    }
    public bool IsExist { get; set; } = true;
}