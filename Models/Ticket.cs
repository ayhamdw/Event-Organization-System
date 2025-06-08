using System.ComponentModel.DataAnnotations.Schema;
using Event_Organization_System.Enums;

namespace Event_Organization_System.model;

public class Ticket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    [ForeignKey("EventId")]
    public Event Event { get; set; }
    
    public DateTime PurchaseDate { get; set; }
    public TicketType Type { get; set; }
    public decimal Price { get; set; }
}