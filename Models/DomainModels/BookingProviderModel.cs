using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.DomainModels
{
    public class BookingProviderModel
    {
        [Key]
        public int BookingProviderId { get; set; }

        public int BookingId { get; set; }
        public BookingModel? Booking { get; set; }

        public int ProviderId { get; set; }
        public ProviderProfileModel? ProviderProfile { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}
