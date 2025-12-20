using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class FilialAreaRepository : IFilialAreaRepository
{
    private readonly AppDbContext _context;
    
    public FilialAreaRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<string> GetFilialName(int filialId)
    {
        return _context.FilialAreas.FirstOrDefault(x => x.Id == filialId).Name;
    }
}