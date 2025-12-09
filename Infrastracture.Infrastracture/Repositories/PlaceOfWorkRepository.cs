using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class PlaceOfWorkRepository: BaseRepository<PlaceOfWork>, IPlaceOfWork
{
    private readonly AppDbContext _context;
    
    public PlaceOfWorkRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<List<PlaceOfWork>> GetByUserId(Guid userId)
    {
        return _context.PlaceOfWork.Where(x => x.UserId == userId).ToList();
    }

    public async Task<List<PlaceOfWork>> GetByOfficeId(Guid officeId)
    {
        return _context.PlaceOfWork.Where(x => x.OfficeId == officeId).ToList();
    }
}