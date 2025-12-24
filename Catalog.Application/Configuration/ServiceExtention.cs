using FluentValidation;
using System.Reflection;
using Infrastracture.Application.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application.Configuration;

public static class ServiceExtention
{
    public static void ConfigureApplicationApp(this IServiceCollection services)
    {
        // services.AddAutoMapper(cfg => {},typeof(UserIntoUserInfoDto).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}