using System.Net;
using System.Security.Claims;
using DTOs;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();


builder.Services
    .AddAuthorization()
    .AddKeycloakAuthorization()
    .AddAuthorizationBuilder();


//порезать при деплое
builder.Services.AddSingleton<HttpMessageHandler>(_ =>
    new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddSingleton(sp =>
    new HttpClient(sp.GetRequiredService<HttpMessageHandler>()));
//


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = true;
        o.Audience = builder.Configuration["Authentication:Audience"];
        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
        };
        
        o.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        }; //при деплое необходимо убрать, т.к. этот параметр позволяет игнорировать, что SSL сертификат игнорируется
    });

builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<UserCheckAuthRequestDto>();
    x.AddRequestClient<RegisterIntoInfrastructureDto>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://ryqfbrei:ZzSKvw_5rVinY_QLFwQ3evnA2EJgogn4@kebnekaise.lmq.cloudamqp.com/ryqfbrei");
        cfg.Message<UserCheckAuthRequestDto>(x => x.SetEntityName("check-auth-queue"));
        cfg.Message<RegisterIntoInfrastructureDto>(x => x.SetEntityName("register-queue"));
    });
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();
