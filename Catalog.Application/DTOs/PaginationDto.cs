namespace Catalog.Application.DTOs;

public record PaginationDto(
    int PageIndex,
    int TotalRecords,
    int TotalPages);