using TicketService.Application.DTOs;

namespace TicketService.Application.HandlerResponse;

public class Response
{
    public string Message { get;  }
    public int Status { get; }
    public GetTicketPanelDto GetTicketPanelDto { get; set; }
    
    
    public Response(string message, int status)
    {
        Message = message;
        Status = status;
    }
    public Response(string message, int status,  GetTicketPanelDto getTicketPanelDto)
    {
        Message = message;
        Status = status;
        GetTicketPanelDto = getTicketPanelDto;
    }
}