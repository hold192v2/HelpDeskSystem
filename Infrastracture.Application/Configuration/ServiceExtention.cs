using System.Reflection;
using FluentValidation;
using Infrastracture.Application.Mappers;
using Infrastracture.Application.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture.Application.Configuration;

public static class ServiceExtensions
{
    public static void ConfigureApplicationApp(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => {},typeof(UserIntoUserInfoDto).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}