using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class ServicePageViewModel
    {
        public int? SelectedCategoryId { get; set; }
        public List<SelectListItem> CategoryOptions { get; set; } = new List<SelectListItem>();
        public List<ServiceIndexViewModel> Services { get; set; } = new List<ServiceIndexViewModel>();
    }
}
