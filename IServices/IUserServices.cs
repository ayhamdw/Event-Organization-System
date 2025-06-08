namespace Event_Organization_System.IServices;

public interface IUserServices
{
    Task<bool> IsUserExists(string email);
    Task<bool> IsUserBookedTicket(int userId, int eventId);
}