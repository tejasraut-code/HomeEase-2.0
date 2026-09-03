using Microsoft.VisualBasic;

namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class BookingIndexViewModel
    {
        public int BookingId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DurationViewModel DurationView { get; set; } = new DurationViewModel();
        public DateTime CreateAt { get; set; }
        public DateTime ScheduledFor { get; set; }
        public string ServiceAddress { get; set; } = string.Empty;
        public string? CustomerNote { get; set; } = string.Empty;
        public string BookingStatus { get; set; } = string.Empty;
    }
}
