namespace Yarp_API_Gateway.Extentions.ExtentionInterfaces;

public interface ICheckedExpiration
{ 
    Task<IRefreshToken> CheckExpirationAsync();
}