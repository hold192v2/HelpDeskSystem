namespace TicketService.Application.DTOs;

public class GetTicketPanelDto
{
    public GetTicketPanelDto(List<ContentTicketModel> content, PaginationModel  pagination)
    {
        Content = content;
        Pagination = pagination;
    }

    public List<ContentTicketModel> Content{ get; set; }
    public PaginationModel Pagination{ get; set; }
    public class ContentTicketModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = "";
        public string Theme { get; set; } = "";
        public string Description { get; set; } = "";
        public string Office { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime DueAt { get; set; }
        public bool IsExpired { get; set; }
    }

    public class PaginationModel
    {
        public int PageIndex { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages  { get; set; }
    }
}