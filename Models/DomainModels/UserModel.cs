using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.DomainModels
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage ="Mobile Number must be exactly 10 digits.")]
        public string Mobile { get; set; } = string.Empty;
        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Address { get; set; } = string.Empty;
        [Required]        
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}
