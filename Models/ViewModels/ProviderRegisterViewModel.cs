using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class ProviderRegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string EmailId { get; set; } = string.Empty;
        [MaxLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage ="Mobile Number must be exactly 10 digits.")]
        public string Mobile { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;


        [Required]
        [Range(0,50)]
        public int ExperienceYears { get; set; }
        [MaxLength(500)]
        public string? Bio { get; set; }
        [Required]
        [MaxLength(100)]
        public string ServiceArea { get; set; } = string.Empty;


        [MinLength(1, ErrorMessage ="Select at least one service.")]
        public List<int> SelectedServiceIds { get; set; } = new List<int>();
        public List<SelectListItem> ServiceOptions { get; set; } = new List<SelectListItem>();
    }
}
