namespace Catalog.Application.DTOs;

public record CategoriesDto(
    List<ContentDto> Content,
    PaginationDto Pagination);