using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class BookingViewModel
    {
        [Required]
        public int ServiceId { get; set; }

        [MaxLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public DurationViewModel DurationView { get; set; } = new DurationViewModel();
        [Required]
        public DateTime ScheduledFor { get; set; }

        [Required]
        [MaxLength(300)]
        public string ServiceAddress { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? CustomerNote { get; set; } = string.Empty;
    }
}
