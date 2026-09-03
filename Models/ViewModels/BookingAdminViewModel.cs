namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class BookingAdminViewModel
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DurationViewModel DurationView { get; set; } = new DurationViewModel();
        public DateTime CreatedAt { get; set; }
        public DateTime ScheduledFor { get; set; }
        public string ServiceAddress { get; set; } = string.Empty;
        public string? CustomerNote { get; set; } = string.Empty;
        public string BookingStatus { get; set; } = string.Empty;
    }
}
