using EventOrganizationSystem.ViewModels;
using EventOrganizationSystem.ViewModels.Responses;

namespace EventOrganizationSystem.IServices;

public interface ITicketServices
{
    Task <bool> BookSeatAsync(BookTicketViewModel bookTicketViewModel , int userId );
    Task<bool> CancelTicketAsync(int ticketId, int userId);
    Task<List<PersonalTicketResponseViewModel>> GetMyBookingAsync(int userId);
}