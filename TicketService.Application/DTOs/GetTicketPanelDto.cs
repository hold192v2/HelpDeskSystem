namespace TicketService.Application.DTOs;

public class GetTicketPanelDto
{
    public GetTicketPanelDto(List<ContentTicketModel> content, PaginationModel  pagination)
    {
        Content = content;
        Pagination = pagination;
    }

    public List<ContentTicketModel> Content { get; set; }
    public PaginationModel Pagination{ get; set; }
}