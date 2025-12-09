using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Yarp_API_Gateway.Extentions;

public class AccessTokenTransformProvider : ITransformProvider //кейс с прикреплением access токена в последующие запросы. Перед тем как отправить, нужно проверить, не просрочен ли токен.
{
    public void ValidateRoute(TransformRouteValidationContext context) { }

    public void ValidateCluster(TransformClusterValidationContext context) { }

    public void Apply(TransformBuilderContext context)
    {
        context.AddRequestTransform(async transformContext =>
        {
            var access = await transformContext.HttpContext.GetTokenAsync("access_token");
            var refresher = new TokenRefresher(
                transformContext.HttpContext.RequestServices.GetRequiredService<IConfiguration>(),
                transformContext.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>());
            var tokens = await (await (await (await (await 
                  TokenRefresher.Start(refresher)
                        .UploadContextAsync(transformContext.HttpContext))
                        .CheckExpirationAsync())
                        .RefreshTokenAsyns())
                        .UploadIntoMiddlewareAsync())
                        .ExecuteAsync();
            
            if (!string.IsNullOrEmpty(access))
            {
                transformContext.ProxyRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", access);
            }
        });
    }
}