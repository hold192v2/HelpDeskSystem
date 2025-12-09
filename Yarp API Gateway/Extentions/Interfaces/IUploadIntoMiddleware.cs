namespace Yarp_API_Gateway.Extentions.ExtentionInterfaces;

public interface IUploadIntoMiddleware
{
    Task<IExecuteStage> UploadIntoMiddlewareAsync();
}