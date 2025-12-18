using AutoMapper;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

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