namespace Yarp_API_Gateway.Extentions.ExtentionInterfaces;

public interface IUploadContext
{
    Task<ICheckedExpiration> UploadContextAsync(HttpContext httpContext);
}