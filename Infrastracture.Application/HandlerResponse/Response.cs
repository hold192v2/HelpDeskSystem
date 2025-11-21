using Infrastracture.Application.DTOs;
using Infrastracture.Application.UseCases.UserInfo;

namespace Infrastracture.Application.HandlerResponse;

public class Response
{
    public string Message { get; set; }
    public int Status { get; set; }
    public UserInfoDTO? UserInfo { get; set; }
    public UserPanelDTO? UserPanel { get; set; }
    

    public Response(string message, int status)
    {
        Message = message;
        Status = status;
    }
    
    public Response(string message, int status, UserInfoDTO request)
    {
        Message = message;
        Status = status;
        UserInfo = request;
    }
    
    public Response(string message, int status, UserPanelDTO request)
    {
        Message = message;
        Status = status;
        UserPanel = request;
    }
}