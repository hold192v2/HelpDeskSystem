using Microsoft.EntityFrameworkCore;
using TicketService.Domain.Entities;
using TicketService.Domain.Interfaces;
using TicketService.Infrastructure.Context;

namespace TicketService.Infrastructure.Repositories;

public class PhotoConnectionRepository :  IPhotoConnectionRepository
{
    private readonly AppDbContext _appDbContext;

    public PhotoConnectionRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task CreateAsync(List<PhotoConnection> photoConnections)
    {
        await _appDbContext.PhotoConnections.AddRangeAsync(photoConnections);
    }

    public async Task<List<PhotoConnection>> GetByTicketIdAsync(Guid ticketId)
    {
        return await _appDbContext.PhotoConnections.Where(photo => photo.TicketId == ticketId).ToListAsync();
    }
}