using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

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
        return (await _context.FilialAreas.FirstOrDefaultAsync(x => x.Id == filialId)).Name;
    }

    public async Task<List<FilialArea>> GetAllFilials()
    {
        return await _context.FilialAreas.ToListAsync();
    }
}