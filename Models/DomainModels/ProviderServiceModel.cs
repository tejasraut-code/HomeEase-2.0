using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.DomainModels
{
    public class ProviderServiceModel
    {
        [Key]
        public int ProviderServiceId { get; set; }

        public int ProviderId { get; set; }
        public ProviderProfileModel? ProviderProfile { get; set; }

        public int ServiceId { get; set; }
        public ServiceModel? Service { get; set; }
    }
}
