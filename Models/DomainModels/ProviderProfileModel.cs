using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.DomainModels
{
    public class ProviderProfileModel
    {
        [Key]
        public int ProviderId { get; set; }

        [Required]
        public int UserId { get; set; }
        public UserModel? User { get; set; }

        [Required]
        [Range(0,50)]    
        public int ExperienceYears { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }

        [Required]
        [MaxLength(100)]
        public string ServiceArea { get; set; } = string.Empty;

        [Required]
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
