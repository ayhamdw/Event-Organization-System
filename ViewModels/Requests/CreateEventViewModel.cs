using System.ComponentModel.DataAnnotations;

namespace EventOrganizationSystem.ViewModels;

public class CreateEventViewModel
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime Time { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public int TotalSeats { get; set; }
    [Required]
    public int RemainingSeats { get; set; }
    [Required]
    public int CreatedBy { get; set; }
}