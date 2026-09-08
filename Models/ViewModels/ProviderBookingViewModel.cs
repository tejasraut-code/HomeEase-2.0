namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class ProviderBookingViewModel
    {
        public int BookingId { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public string ServiceAddress { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public string? CustomerNote { get; set; }

        public DateTime BookedOn { get; set; }
        public DateTime ScheduledFor { get; set; }

        public decimal ServicePrice { get; set; }

        public string BookingStatus { get; set; } = string.Empty;
    }
}
