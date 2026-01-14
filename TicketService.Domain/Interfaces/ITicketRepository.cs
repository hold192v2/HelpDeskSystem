using TicketService.Domain.Entities;

namespace TicketService.Domain.Interfaces;

public interface ITicketRepository
{
    Task Create(Ticket ticket); //TODO поправить на корректное отображение
    Task<List<Ticket>> GetTicketsForPanelWithSearch(int page, int pageSize, int? priorityId, int? statusId, int? dateSort, string theme);
}