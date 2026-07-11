using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.DomainModels
{
    public class CategoryModel
    {
        [Key] //primary key
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Image { get; set; } = string.Empty;
    }
}
