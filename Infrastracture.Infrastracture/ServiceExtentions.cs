using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Infrastracture.Infrastracture.Repositories;
using Infrastracture.Infrastracture.Seeds.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture.Infrastracture;

public static class ServiceExtentions
{
    public static void ConfigurePresistanceApp(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres");
        IServiceCollection serviceCollection = services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString, x => x.MigrationsAssembly("Infrastructure.Infrastructure")), ServiceLifetime.Scoped);
        
        serviceCollection.AddScoped<IFilialArea, FilialAreaRepository>();
        serviceCollection.AddScoped<IOffice, OfficeRepository>();
        serviceCollection.AddScoped<IRegion, RegionRepository>();
        serviceCollection.AddScoped<IRole, RoleRepository>();
        serviceCollection.AddScoped<IUser, UserRepository>();
        serviceCollection.AddScoped<IPlaceOfWork, PlaceOfWorkRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<SeedInitializer>();
    }
}