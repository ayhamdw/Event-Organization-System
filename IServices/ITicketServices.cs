using Event_Organization_System.ViewModels;

namespace Event_Organization_System.IServices;

public interface ITicketServices
{
    Task <bool> BookSeatAsync(BookTicketViewModel bookTicketViewModel , int userId );
    Task<bool> CancelTicketAsync(int ticketId, int userId);
}