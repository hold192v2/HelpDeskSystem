using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class BaseRepository<T>: IBaseOperationRepository<T> where T : BaseEntity
{
    private readonly AppDbContext appDbContext;

    public BaseRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }
    public void Create(T entity)
    {
        appDbContext.Add(entity);
    }

    public void Delete(T entity)
    {
        appDbContext.Remove(entity);
    }

    public List<T> GetAll()
    {
        return appDbContext.Set<T>().ToList();
    }

    public void Update(T entity)
    {
        appDbContext.Update(entity);
    }
}