using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.DomainModels
{
    public class BookingModel
    {
        [Key]
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public UserModel? User { get; set; }
        public int ServiceId { get; set; }
        public ServiceModel? Service { get; set; }

        [Required]
        [MaxLength(100)]
        public string? ServiceNameAtBooking { get; set; } = string.Empty;


        public decimal PriceAtBooking { get; set; }
        public int? DurationMinutesAtBooking { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ScheduledFor { get; set; }


        [Required]
        [MaxLength(300)]
        public string ServiceAddress { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? CustomerNote { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string BookingStatus { get; set; } = "Pending";


    }
}
