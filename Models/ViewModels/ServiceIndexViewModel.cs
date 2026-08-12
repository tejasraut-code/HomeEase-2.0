namespace HomeEase_2._0_MVC.Models.ViewModels
{
    public class ServiceIndexViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ExistingImage { get; set; } 
        public DurationViewModel DurationView { get; set; } = new DurationViewModel();


    }
}


// serviceName , Description , price, Days, Hours, Minutes, ExistingImage, optional: CategroyName