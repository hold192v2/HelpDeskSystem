using Yarp_API_Gateway.DTOs;

namespace Yarp_API_Gateway.Extentions.ExtentionInterfaces;

public interface IExecuteStage
{
    Task<SessionDTO> ExecuteAsync();
}