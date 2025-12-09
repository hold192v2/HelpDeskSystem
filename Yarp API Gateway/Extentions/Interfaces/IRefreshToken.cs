namespace Yarp_API_Gateway.Extentions.ExtentionInterfaces;

public interface IRefreshToken
{
    Task<IUploadIntoMiddleware> RefreshTokenAsyns();
}