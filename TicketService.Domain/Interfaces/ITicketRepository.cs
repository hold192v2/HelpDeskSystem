using TicketService.Domain.Entities;

namespace TicketService.Domain.Interfaces;

public interface ITicketRepository
{
    Task Create(Ticket ticket); //TODO поправить на корректное отображение
    Task<List<Ticket>> GetTicketsForPanelWithSearch(IQueryable<Ticket> query, int page, int pageSize, int? priorityId, int? statusId, int? dateSort, string theme);
    Task<int> CountAsync(IQueryable<Ticket> query, int? priorityId, int? statusId, int? dateSort, string theme);
    IQueryable<Ticket> Query();
    Task<Dictionary<int, List<Guid>>> PerformerTicketCount(List<Guid> performerIds);
}