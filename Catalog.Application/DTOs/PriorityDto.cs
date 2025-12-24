namespace Catalog.Application.DTOs;

public record PriorityDto(
    int PriorityId,
    string Name,
    double Sla);