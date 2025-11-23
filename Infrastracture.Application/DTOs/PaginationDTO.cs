namespace Infrastracture.Application.DTOs;

public class PaginationDTO
{
    public int PageIndex { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}