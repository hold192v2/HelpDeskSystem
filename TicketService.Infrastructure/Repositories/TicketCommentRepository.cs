using TicketService.Domain.Entities;
using TicketService.Domain.Interfaces;
using TicketService.Infrastructure.Context;

namespace TicketService.Infrastructure.Repositories;

public class TicketCommentRepository : ITicketCommentRepository
{
    private readonly AppDbContext _appDbContext;

    public TicketCommentRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public Task<TicketComment> GetTicketCommentByIdAsync(long ticketCommentId)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync(TicketComment ticketComment)
    {
        await _appDbContext.TicketComments.AddAsync(ticketComment);
    }
}