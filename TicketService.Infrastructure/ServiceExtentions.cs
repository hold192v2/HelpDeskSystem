using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketService.Domain.Interfaces;
using TicketService.Infrastructure.Context;
using TicketService.Infrastructure.Repositories;

namespace TicketService.Infrastructure;

public static class ServiceExtentions
{
    public static void ConfigurePresistanceApp(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres");
        IServiceCollection serviceCollection = services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString, x => x.MigrationsAssembly("Infrastructure.Infrastructure")), ServiceLifetime.Scoped);
        
        serviceCollection.AddScoped<IFeedbackRepository, FeedbackRepository>();
        serviceCollection.AddScoped<IReportTicketRepository, ReportTicketRepository>();
        serviceCollection.AddScoped<ITicketCommentRepository, TicketCommentRepository>();
        serviceCollection.AddScoped<ITicketRepository, TicketRepository>();
        serviceCollection.AddScoped<ITicketPauseLogRepository, TicketPauseLogRepository>();
        serviceCollection.AddScoped<ITicketStatusChangeLogRepository, TicketStatusChangeLogRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}