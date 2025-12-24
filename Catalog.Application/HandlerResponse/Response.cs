using Catalog.Application.DTOs;

namespace Catalog.Application.HandlerResponse;

public class Response
{
    public string Message { get; set; }
    public int Status { get; set; }
    public CategoriesDto? Categories { get; set; }
    public SpecificCategoryDto? SpecificCategory { get; set; }
    public List<PriorityDto>? Priorities { get; set; }

    public Response(string message, int status)
    {
        Message = message;
        Status = status;
    }

    public Response(string message, int status, CategoriesDto? categories)
    {
        Message = message;
        Status = status;
        Categories = categories;
    }

    public Response(string message, int status, SpecificCategoryDto? specificCategory)
    {
        Message = message;
        Status = status;
        SpecificCategory = specificCategory;
    }

    public Response(string message, int status, List<PriorityDto>? priorities)
    {
        Message = message;
        Status = status;
        Priorities = priorities;
    }
}