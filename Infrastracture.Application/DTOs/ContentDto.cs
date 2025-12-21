namespace Infrastracture.Application.DTOs;

public record ContentDto(
    Guid Id,
    string Name,
    string Surname,
    string Patronymic,
    string SystemId,
    string Email,
    double Rating,
    List<string> Category,
    List<string> Office);
