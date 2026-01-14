using TicketService.Domain.Interfaces;
using TicketService.Infrastructure.Context;

namespace TicketService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext appDbContext;

    public UnitOfWork(AppDbContext context)
    {
        appDbContext = context;
    }
    public async Task Commit(CancellationToken cancellationToken)
    {
        await appDbContext.SaveChangesAsync();
    }
}