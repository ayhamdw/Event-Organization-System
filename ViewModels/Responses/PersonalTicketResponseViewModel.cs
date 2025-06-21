namespace EventOrganizationSystem.ViewModels.Responses;

public class PersonalTicketResponseViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Time  { get; set; }


    public PersonalTicketResponseViewModel(string title, string description, string location, DateTime time)
    {
        Title = title;
        Description = description;
        Location = location;
        Time = time;
    }
}