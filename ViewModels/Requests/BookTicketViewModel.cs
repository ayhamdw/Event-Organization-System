namespace Event_Organization_System.ViewModels;

public class BookTicketViewModel
{
    public int UserId { get; set; }
    public int EventId { get; set; }
    public decimal Price { get; set; }
    public string TicketType { get; set; }
    public DateTime PurchaseDate { get; set; }
    
}