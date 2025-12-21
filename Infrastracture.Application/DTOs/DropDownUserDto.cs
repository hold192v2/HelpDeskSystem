namespace Infrastracture.Application.DTOs;

public record DropDownUserDto(
    Guid UserId, 
    string Name, 
    string Surname, 
    string Patronymic, 
    string Email,
    double? Rating);