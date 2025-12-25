using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;

namespace Catalog.Infrastructure.Repositories;

public class UnitOfWork: IUnitOfWork
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