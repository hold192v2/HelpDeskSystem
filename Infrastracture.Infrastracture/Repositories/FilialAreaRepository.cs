using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class FilialAreaRepository: BaseRepository<Domain.Entities.FilialArea>, IFilialArea
{
    private readonly AppDbContext _context;
    
    public FilialAreaRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<string> GetFilialName(int filialId)
    {
        return _context.FilialAreas.FirstOrDefault(x => x.Id == filialId).Name;
    }
}