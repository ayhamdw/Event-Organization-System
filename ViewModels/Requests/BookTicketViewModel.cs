namespace EventOrganizationSystem.ViewModels;

public class BookTicketViewModel
{
    public int UserId { get; set; }
    public int EventId { get; set; }
    public decimal Price { get; set; }
    public string TicketType { get; set; }
}