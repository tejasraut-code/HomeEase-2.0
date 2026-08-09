using HomeEase_2._0_MVC.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class ServiceViewModel
    {
        public int ServiceId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }
        public TimeSpan ApproximateTime { get; set; }
        public IFormFile? BrowserImage { get; set; } 
        public string ExistingImage { get; set; } = string.Empty;
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    }
}
