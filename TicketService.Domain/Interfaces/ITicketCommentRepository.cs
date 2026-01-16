using TicketService.Domain.Entities;

namespace TicketService.Domain.Interfaces;

public interface ITicketCommentRepository
{
    Task<TicketComment> GetTicketCommentByIdAsync(long ticketCommentId);
    Task CreateAsync(TicketComment ticketComment);
}