using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TicketService.Domain.Entities;

namespace TicketService.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    public DbSet<CrossingTicket> CrossingTickets { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Domain.Entities.Ticket> Tickets { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<TicketPauseLog> TicketPauseLogs { get; set; }
    public DbSet<TicketStatusChangeLog> TicketStatusChangeLogs { get; set; }
}

public class YourDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=helpdesk_ticket_service;Username=postgres;Password=second");

        return new AppDbContext(optionsBuilder.Options);
    }
}