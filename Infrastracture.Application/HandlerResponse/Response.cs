using Infrastracture.Application.DTOs;
using Infrastracture.Application.UseCases.UserInfo;

namespace Infrastracture.Application.HandlerResponse;

public class Response
{
    public string Message { get; set; }
    public int Status { get; set; }
    public UserInfoDTO? UserInfo { get; set; }
    public UserPanelDTO? UserPanel { get; set; }
    public PerformersDTO? Performers { get; set; }
    public List<OfficeDTO>? Offices { get; set; }
    public List<UserDTO>? Users { get; set; }
    public List<RegionDTO>? Regions { get; set; }

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

    public Response(string message, int status, PerformersDTO request)
    {
        Message = message;
        Status = status;
        Performers = request;
    }

    public Response(string message, int status, List<OfficeDTO> request)
    {
        Message = message;
        Status = status;
        Offices = request;
    }

    public Response(string message, int status, List<UserDTO> request)
    {
        Message = message;
        Status = status;
        Users = request;
    }

    public Response(string message, int status, List<RegionDTO> request)
    {
        Message = message;
        Status = status;
        Regions = request;
    }
}