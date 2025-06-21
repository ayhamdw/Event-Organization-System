using System.ComponentModel.DataAnnotations.Schema;
using EventOrganizationSystem.Enums;

namespace EventOrganizationSystem.model;

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
    public TicketStatus Status { get; set; }
    

    public Ticket(int userId, int eventId, TicketType type, decimal price)
    {
        UserId = userId;
        EventId = eventId;
        Type = type;
        Price = price;
        PurchaseDate = DateTime.UtcNow;
        Status = TicketStatus.Active;
    }
}