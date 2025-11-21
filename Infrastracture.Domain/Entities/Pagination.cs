namespace Infrastracture.Domain.Entities;

public class Pagination
{
    public int pageIndex { get; set; }
    public int totalRecords { get; set; }
    public int totalPages { get; set; }
}