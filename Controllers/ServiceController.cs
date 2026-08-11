using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ServiceController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ServiceViewModel viewService = new ServiceViewModel();

            List<CategoryModel> categories = _context.Category.ToList();
            viewService.CategoryOptions = new SelectList(categories, "CategoryId", "CategoryName");

            return View(viewService);
        }
        [HttpPost]
        public IActionResult Create(ServiceViewModel viewService)
        {
            if (ModelState.IsValid)
            {
                ServiceModel Service = new ServiceModel();
                string servicename = viewService.ServiceName.Trim().ToLower();

                int categroyId = viewService.CategoryId;
                CategoryModel? category = _context.Category.Find(categroyId);

                var serviceNamePresent = _context.Services.Any( x => x.ServiceName.ToLower() == servicename);

                if(serviceNamePresent)
                {
                    ViewBag.ErrorMessage = "Service Name Already Present.";
                    return View(viewService);
                }

                if(category == null)
                {
                    return NotFound(viewService);
                }
                Service.CategoryId = viewService.CategoryId;
                Service.Description = viewService.Description;
                Service.ServiceName = viewService.ServiceName.Trim();
                Service.Price = viewService.Price;
                Service.EstimatedDurationMinutes = DurationToMinutesCal(viewService.DurationDays, viewService.DurationHours, viewService.DurationMinutes);
                Service.RequiredSiteVisit = viewService.RequiredSiteVisit;
                Service.DurationNote = viewService.DurationNote;

                if (viewService.BrowserImage != null)
                {
                    var imageName = UploadImage(viewService.BrowserImage);
                    Service.Image = imageName;
                }
                else
                {
                    Service.Image = null;
                }

                _context.Services.Add(Service);
                _context.SaveChanges();

                return RedirectToAction("Create");
            }

            return View(viewService);
        }

        private int DurationToMinutesCal(int Days,int Hours, int Minutes)
        {
            int DurationInMinutes = ( Days*1440)+(Hours*60)+(Minutes*1);
            return DurationInMinutes;
        }
        private string UploadImage(IFormFile BrowserImage)
        {
            var fileName = BrowserImage.FileName ;
            var folderPath = Path.Combine(_environment.WebRootPath, "images");
            var fileUploadPath = Path.Combine(folderPath, fileName);

            using(var stream = new FileStream(fileUploadPath, FileMode.Create))
            {
                BrowserImage.CopyTo(stream);
            }
            return fileName;
        } 

    }
}
