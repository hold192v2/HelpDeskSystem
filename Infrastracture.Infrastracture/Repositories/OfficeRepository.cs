using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;

namespace Infrastracture.Infrastracture.Repositories;

public class OfficeRepository: BaseRepository<Domain.Entities.Office>, IOffice
{
    private readonly AppDbContext _context;
    
    public OfficeRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<Domain.Entities.Office> GetOfficeById(Guid officeId)
    {
        return _context.Offices.FirstOrDefault(x => x.Id == officeId);
    }

    public void AddOffice(string city, string adress, int regionId)
    {
        var office = new Domain.Entities.Office();
        office.City = city;
        office.Address = adress;
        office.RegionId = regionId;
        _context.Offices.Add(office);
    }

    public void EditOffice(Guid officeId, string city, string adress)
    {
        var office = _context.Offices.FirstOrDefault(x => x.Id == officeId);
        office.City = city;
        office.Address = adress;
    }

    public async Task<List<Domain.Entities.Office>> GetOfficesByRegionId(int regionId)
    {
        return _context.Offices.Where(x => x.RegionId == regionId).ToList();
    }
}