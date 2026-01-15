using TicketService.Domain.Entities;

namespace TicketService.Application.Extentions.ExtentionMethods;

public static class GroupInitializer
{
    public static TicketSortGroup ResolveGroup(Ticket ticket, DateTime now)
    {
        if ((StatusEnum)ticket.StatusId == StatusEnum.Completed || (StatusEnum)ticket.StatusId == StatusEnum.Rejected)
            return TicketSortGroup.Final;

        if (ticket.DueAt < now)
            return TicketSortGroup.Expired;

        if ((ticket.DueAt - now).TotalHours <= 24)
            return TicketSortGroup.Urgent;

        return TicketSortGroup.Normal;
    }
}