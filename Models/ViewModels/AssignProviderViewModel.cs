using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class AssignProviderViewModel
    {
        public int BookingId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        [Range(1,int.MaxValue, ErrorMessage ="Please Select a Provider.")]
        public int SelectedProviderId { get; set; }
        public List<SelectListItem> ProviderOptions { get; set; } = new List<SelectListItem>();
    }
}
