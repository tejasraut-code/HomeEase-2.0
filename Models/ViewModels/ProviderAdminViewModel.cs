namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class ProviderAdminViewModel
    {
        public int ProviderId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string? Bio { get; set; }
        public string ServiceArea { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
    }
}
