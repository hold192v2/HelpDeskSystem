using System.Security.Claims;
using System.Text.Json;
using DTOs;
using Infrastracture.Application.Configuration;
using Infrastracture.Application.DTOs;
using Infrastracture.Infrastracture;
using Infrastracture.Infrastracture.Seeds.Extentions;
using Keycloak.AuthServices.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
            RoleClaimType = "realm_access.roles"
        };
        o.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
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
        };//при деплое необходимо убрать, т.к. этот параметр позволяет игнорировать, что SSL сертификат самоподписанный.
    });

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AuthCheckConsumer>();
    x.AddConsumer<RegisterConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://ryqfbrei:ZzSKvw_5rVinY_QLFwQ3evnA2EJgogn4@kebnekaise.lmq.cloudamqp.com/ryqfbrei");
        cfg.ReceiveEndpoint("check-auth-queue", x =>
        {
            x.ConfigureConsumer<AuthCheckConsumer>(context);
            x.Bind("exchange-name");
        });
        cfg.ReceiveEndpoint("register-queue", x =>
        {
            x.ConfigureConsumer<RegisterConsumer>(context);
            x.Bind("exchange-register-name");
        });

    });

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<SeedInitializer>();
    await initializer.Initialize();
}

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