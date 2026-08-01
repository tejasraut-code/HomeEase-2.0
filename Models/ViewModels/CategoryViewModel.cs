using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public IFormFile? BrowserImage { get; set; }

        public string? ExistingImage { get; set; }
    }
}
