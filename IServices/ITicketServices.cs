using Event_Organization_System.ViewModels;
using Event_Organization_System.ViewModels.Responses;

namespace Event_Organization_System.IServices;

public interface ITicketServices
{
    Task <bool> BookSeatAsync(BookTicketViewModel bookTicketViewModel , int userId );
    Task<bool> CancelTicketAsync(int ticketId, int userId);
    Task<List<PersonalTicketResponseViewModel>> GetMyBookingAsync(int userId);
}