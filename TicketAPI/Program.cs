using System.Security.Claims;
using System.Text.Json;
using Keycloak.AuthServices.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicketService.Application.Configuration;
using TicketService.Application.DTOs;
using TicketService.Application.Mappers;
using TicketService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.ConfigurePresistanceApp(builder.Configuration);
builder.Services.ConfigureApplicationApp();
builder.Services
    .AddAuthorization()
    .AddKeycloakAuthorization()
    .AddAuthorizationBuilder();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = true;
        o.Audience = builder.Configuration["Authentication:Audience"];
        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
        };
        o.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal!.Identity as ClaimsIdentity;

                var realmAccess = context.Principal.FindFirst("realm_access");
                if (realmAccess != null)
                {
                    using var doc = JsonDocument.Parse(realmAccess.Value);
                    if (doc.RootElement.TryGetProperty("roles", out var roles))
                    {
                        foreach (var role in roles.EnumerateArray())
                        {
                            identity!.AddClaim(
                                new Claim(ClaimTypes.Role, role.GetString()!.Trim().ToLower())
                            );
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<AdminOfficesGetRequestDto>();
    x.AddRequestClient<OfficeNameGetRequestDto>();
    x.AddRequestClient<CatalogTicketPanelRequestDto>();
    x.AddRequestClient<CreationTicketCatalogRequestDto>();
    x.AddRequestClient<CreateTicketPerformersIdsGetRequestDto>();
    x.AddRequestClient<TicketIntoInfrastructureRequestDto>();
    x.AddRequestClient<TicketInfoCatalogRequestDto>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://ryqfbrei:ZzSKvw_5rVinY_QLFwQ3evnA2EJgogn4@kebnekaise.lmq.cloudamqp.com/ryqfbrei");
        cfg.Message<AdminOfficesGetRequestDto>(x => x.SetEntityName("admin-offices-queue"));
        cfg.Message<OfficeNameGetRequestDto>(x => x.SetEntityName("ticket-name-offices-queue"));
        cfg.Message<CatalogTicketPanelRequestDto>(x => x.SetEntityName("catalog-ticket-panel-queue"));
        cfg.Message<CreationTicketCatalogRequestDto>(x => x.SetEntityName("catalog-ticket-creation-queue"));
        cfg.Message<CreateTicketPerformersIdsGetRequestDto>(x => x.SetEntityName("performers-ticket-creation-queue"));
        cfg.Message<TicketIntoInfrastructureRequestDto>(x => x.SetEntityName("ticket-infrastructure-info-queue"));
        cfg.Message<TicketInfoCatalogRequestDto>(x => x.SetEntityName("ticket-catalog-info-queue"));
    });

});

var app = builder.Build();

app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "v1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


app.Run();
