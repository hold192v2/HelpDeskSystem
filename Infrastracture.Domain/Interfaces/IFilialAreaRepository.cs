using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IFilialAreaRepository
{
    Task<string> GetFilialName(int filialId);
    public Task<List<FilialArea>> GetAllFilials();
}