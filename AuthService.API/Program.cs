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
    .AddAuthorizationBuilder()
    .AddPolicy(
        "EmployeePolicy",
        policy =>
            policy.RequireRealmRoles(
                "test-client",
                "employee"
            )
    )
    .AddPolicy(
        "PerformerPolicy",
        policy =>
            policy.RequireRealmRoles(
                "test-client",
                "performer"
            )
    )
    .AddPolicy(
        "AdminPolicy",
        policy =>
            policy.RequireRealmRoles(
                "test-client",
                "admin"
            )
    )
    .AddPolicy(
        "AnalystPolicy",
        policy =>
            policy.RequireRealmRoles(
                "test-client",
                "analyst"
            )
    )
    .AddPolicy(
        "SuperadminPolicy",
        policy =>
            policy.RequireRealmRoles(
                "test-client",
                "superadmin"
            )
    );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.Audience = builder.Configuration["Authentication:Audience"];
        o.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
        };
    });

builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<UserCheckAuthRequestDto>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://ryqfbrei:ZzSKvw_5rVinY_QLFwQ3evnA2EJgogn4@kebnekaise.lmq.cloudamqp.com/ryqfbrei");
        cfg.Message<UserCheckAuthRequestDto>(x => x.SetEntityName("check-auth-govno"));
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
    options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["Keycloak:AuthorizationUrl"]!),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "openid" },
                    { "profile", "profile" }
                }
            }
        }
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Keycloak",
                    Type = ReferenceType.SecurityScheme
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            },
            []
        }
    });
    options.CustomSchemaIds(type => type.ToString());
});
var app = builder.Build();

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
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();
