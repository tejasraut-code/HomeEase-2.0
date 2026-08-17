using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.DomainModels
{
    public class ServiceModel
    {
        [Key]
        public int ServiceId { get; set; }

        public int CategoryId { get; set; }
        public CategoryModel? Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        //[Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        //public TimeSpan ApproximateTime { get; set; }
        public int? EstimatedDurationMinutes { get; set; }
        public bool RequiredSiteVisit { get; set; }
        public  string? DurationNote { get; set; }

        public string? Image { get; set; } 
    }
}
