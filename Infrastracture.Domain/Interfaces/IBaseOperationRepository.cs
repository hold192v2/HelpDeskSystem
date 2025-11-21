using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IBaseOperationRepository<T> where T: BaseEntity
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    List<T> GetAll();
}