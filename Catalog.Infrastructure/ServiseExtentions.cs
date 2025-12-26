using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Seeds.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class ServiseExtentions
{
    public static void ConfigurePresistanceApp(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres");
        var serviceCollection = services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString, x => x.MigrationsAssembly("Catalog.Infrastructure")));
        
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddScoped<IPriorityRepository, PriorityRepository>();
        serviceCollection.AddScoped<IStatusRepository, StatusRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<SeedInitializer>();
    }
}