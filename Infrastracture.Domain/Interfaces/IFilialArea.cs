using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IFilialArea: IBaseOperationRepository<FilialArea>
{
    Task<string> GetFilialName(int filialId);
}