namespace Infrastracture.Application.DTOs;

public record ContentDto(
    Guid Id,
    string Name,
    string Surname,
    string Patronymic,
    List<string> Category,
    List<string> Office);
