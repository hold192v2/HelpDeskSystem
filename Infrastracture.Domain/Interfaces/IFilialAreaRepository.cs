using Infrastracture.Domain.Dtos;
using Infrastracture.Domain.Entities;

namespace Infrastracture.Domain.Interfaces;

public interface IFilialAreaRepository
{
    Task<string> GetFilialName(int filialId);
    Task<List<FilialArea>> GetAllFilials();
    Task<FilialArea> GetFilialByFilialId(int filialId);
    Task UpdateAnalystId(Guid userId, int filialId);
    Task<List<FilialDto>> GetFilialsWithAnalystAsync();
}