namespace Infrastracture.Application.DTOs;

public class PaginationDTO
{
    public int pageIndex { get; set; }
    public int totalRecords { get; set; }
    public int totalPages { get; set; }
}