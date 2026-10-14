using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.DTOs;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServiceApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Service(int? categoryId)
        {
            List<ServiceModel> serviceList = await _context.Services.Include(x => x.Category).Where( x => categoryId == null || x.CategoryId == categoryId).ToListAsync();
            List<ServiceDto> serviceDtosList = new List<ServiceDto>();

            foreach (var item in serviceList)
            {
                ServiceDto serviceDto = new ServiceDto();
                serviceDto.ServiceId = item.ServiceId;
                serviceDto.ServiceName = item.ServiceName;
                serviceDto.Description = item.Description;
                serviceDto.Price = item.Price;
                serviceDto.CategoryName = item.Category?.CategoryName ?? "Unknown Category";
                serviceDto.Image = item.Image;
                serviceDto.EstimatedDurationMinutes = item.EstimatedDurationMinutes;
                //serviceDto.DurationView = MinutesToDuration(item.EstimatedDurationMinutes);

                serviceDtosList.Add(serviceDto);
            }
            return Ok(serviceDtosList);
        }

        //public DurationViewModel MinutesToDuration(int? time)
        //{
        //    DurationViewModel durationView = new DurationViewModel();
        //    if(time == null)
        //    {
        //        return durationView;
        //    }

        //    var totaltime = time.Value;
        //    durationView.Days = totaltime / 1440;

        //    var timeremain = totaltime % 1440;
        //    durationView.Hours = timeremain / 60;
        //    durationView.Minutes = timeremain % 60;

        //    return durationView;
        //}
    }

}
