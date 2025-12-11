using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Yarp_API_Gateway.Extentions;
using Yarp.ReverseProxy.Transforms.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddSingleton<ITransformProvider, AccessTokenTransformProvider>();
builder.Services.AddHttpClient("AllowAnyCert")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = 
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.RequireHttpsMetadata = true;
        options.Authority = builder.Configuration["Authentication:ValidIssuer"];
        options.ClientId = builder.Configuration["Keycloak:ClientId"];
        options.ClientSecret = builder.Configuration["Keycloak:ClientSecret"];
        options.ResponseType = "code";
        options.SaveTokens = true; 
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        }; // при деплое необходимо будет вырезать, т.к. здесь происходит игнорирование самодписанного SSL.
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });
    options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.OAuthUsePkce();
    c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "v1");
});
//app.UseHttpsRedirection();
app.UseAuthentication()
    .UseAuthorization();
app.MapControllers();
app.MapReverseProxy()
   .RequireAuthorization();
app.Run();
