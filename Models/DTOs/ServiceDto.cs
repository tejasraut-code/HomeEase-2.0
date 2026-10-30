namespace HomeEase_2._0_MVC.Models.DTOs
    {
        public class ServiceDto
        {
            public int ServiceId { get; set; }
            public string ServiceName { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string? Image { get; set; }
            public int? EstimatedDurationMinutes { get; set; }
            public DurationDto Duration { get; set; } = new DurationDto();  
            public string CategoryName { get; set; } = string.Empty;
        }
    }
