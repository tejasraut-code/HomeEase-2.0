using HomeEase_2._0_MVC.Models.DomainModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        //public TimeSpan ApproximateTime { get; set; }

        public int? EstimatedDurationMinutes { get; set; }
        public int DurationDays { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public bool RequiredSiteVisit { get; set; }
        public string? DurationNote { get; set; }



        public IFormFile? BrowserImage { get; set; } 
        public string? ExistingImage { get; set; } = string.Empty;
        public SelectList? CategoryOptions { get; set; }

        public DurationViewModel DurationView { get; set; } = new DurationViewModel();
    }
}
