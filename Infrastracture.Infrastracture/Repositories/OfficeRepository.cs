using Infrastracture.Domain.Entities;
using Infrastracture.Domain.Interfaces;
using Infrastracture.Infrastracture.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Infrastracture.Repositories;

public class OfficeRepository: IOfficeRepository
{
    private readonly AppDbContext _context;
    
    public OfficeRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<Office> GetOfficeByIdAsync(Guid officeId)
    {
        return await _context.Offices.FirstOrDefaultAsync(x => x.Id == officeId);
    }

    public async Task AddOfficeAsync(string city, string address, int regionId)
    {
        var office = new Office();
        office.City = city;
        office.Address = address;
        office.RegionId = regionId;
        await _context.Offices.AddAsync(office);
    }

    public async Task EditOfficeAsync(Guid officeId, string city, string adress)
    {
        var office = await _context.Offices.FirstOrDefaultAsync(x => x.Id == officeId);
        office!.City = city;
        office.Address = adress;
    }

    public Office GetOfficeById(Guid officeId)
    {
        return  _context.Offices.FirstOrDefault(x => x.Id == officeId);
    }
    

    public async Task<List<Office>> GetOfficesByRegionIdAsync(int regionId)
    {
        return _context.Offices.Where(x => x.RegionId == regionId).ToList();
    }
}