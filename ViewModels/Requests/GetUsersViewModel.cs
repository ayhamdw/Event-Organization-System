namespace Event_Organization_System.ViewModels.Requests
{
    public class GetUsersViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
    }
}