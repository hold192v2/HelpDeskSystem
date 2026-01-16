using TicketService.Domain.Entities;

namespace TicketService.Domain.Interfaces;

public interface IPhotoConnectionRepository
{
    Task CreateAsync(List<PhotoConnection> photoConnection);
    Task<List<PhotoConnection>> GetByTicketIdAsync(Guid ticketId);
}