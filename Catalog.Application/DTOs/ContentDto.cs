namespace Catalog.Application.DTOs;

public record ContentDto(
    Guid Id,
    string Name,
    string Description,
    double Sla);