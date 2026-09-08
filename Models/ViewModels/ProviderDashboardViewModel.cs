namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class ProviderDashboardViewModel
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string ServiceArea { get; set; } = string.Empty;
        public string? Bio { get; set; }

        public List<string> ServiceNames { get; set; } = new List<string>();

        public List<ProviderBookingViewModel> ProviderBookingView { get; set; } = new List<ProviderBookingViewModel>();
    }
}
