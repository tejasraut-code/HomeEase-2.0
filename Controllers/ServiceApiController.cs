using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.DTOs;
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
        public IActionResult Service(int? categoryId)
        {
            List<ServiceModel> serviceList = _context.Services.Include(x => x.Category).Where( x => categoryId == null || x.CategoryId == categoryId).ToList();
            List<ServiceDto> serviceDtosList = new List<ServiceDto>();

            foreach (var item in serviceList)
            {
                ServiceDto serviceDto = new ServiceDto();
                serviceDto.ServiceId = item.ServiceId;
                serviceDto.ServiceName = item.ServiceName;
                serviceDto.Description = item.Description;
                serviceDto.Price = item.Price;
                serviceDto.CategoryName = item.Category?.CategoryName ?? "Unknown Category";

                serviceDtosList.Add(serviceDto);
            }
            return Ok(serviceDtosList);
        }
    }

}
