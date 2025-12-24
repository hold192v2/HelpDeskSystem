namespace Catalog.Application.DTOs;

public record SpecificCategoryDto(
    Guid Id,
    string Name,
    string Description,
    int Sla);