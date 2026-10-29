using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            string categoryName = "All Services";
            if (categoryId != null)
            {
                CategoryModel? category = await _context.Category.FindAsync(categoryId.Value);
                if (category == null)
                {
                    return NotFound();
                }
                categoryName = category.CategoryName;
            }

            ServicePageViewModel servicePageView = new ServicePageViewModel();
            servicePageView.SelectedCategoryId = categoryId;
            servicePageView.CategoryOptions = await _context.Category.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString()
            }).ToListAsync();

            List<ServiceModel> serviceViews = await _context.Services.Include( x => x.Category).Where(x => categoryId == null || x.CategoryId == categoryId).ToListAsync();
            foreach( var service in serviceViews)
            {
                ServiceIndexViewModel serviceIndexView = new ServiceIndexViewModel();
                serviceIndexView.ServiceId = service.ServiceId;
                serviceIndexView.ServiceName = service.ServiceName;
                serviceIndexView.Description = service.Description;
                serviceIndexView.Price = service.Price;
                serviceIndexView.ExistingImage = service.Image;
                serviceIndexView.DurationView = MinutesToDuration(service.EstimatedDurationMinutes);
                serviceIndexView.CategoryName = service.Category?.CategoryName;

                servicePageView.Services.Add(serviceIndexView);
            }

            ViewBag.categoryName = categoryName;
            return View(servicePageView);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ServiceViewModel viewService = new ServiceViewModel();

            List<CategoryModel> categories = await _context.Category.ToListAsync();
            viewService.CategoryOptions = new SelectList(categories, "CategoryId", "CategoryName");

            return View(viewService);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ServiceViewModel viewService)
        {
            List<CategoryModel> categories = await _context.Category.ToListAsync();
            viewService.CategoryOptions = new SelectList(categories, "CategoryId", "CategoryName");

            if (ModelState.IsValid)
            {
                ServiceModel Service = new ServiceModel();
                string servicename = viewService.ServiceName.Trim().ToLower();

                int categoryId = viewService.CategoryId;
                CategoryModel? category = await _context.Category.FindAsync(categoryId);

                var serviceNamePresent = await _context.Services.AnyAsync( x => x.ServiceName.ToLower() == servicename);

                if(serviceNamePresent)
                {
                    //ViewBag.ErrorMessage = "Service Name Already Present.";
                    ModelState.AddModelError(nameof(viewService.ServiceName), "Service Name Already Present. ");
                    return View(viewService);
                }

                if(category == null)
                {
                    //return NotFound(viewService);\
                    ModelState.AddModelError(nameof(viewService.CategoryId), "Please select a valid category.");
                    return View(viewService);
                }
                Service.CategoryId = viewService.CategoryId;
                Service.Description = viewService.Description;
                Service.ServiceName = viewService.ServiceName.Trim();
                Service.Price = viewService.Price;
                Service.EstimatedDurationMinutes = DurationToMinutes(viewService.DurationView.Days, viewService.DurationView.Hours, viewService.DurationView.Minutes);
                Service.RequiredSiteVisit = viewService.RequiredSiteVisit;
                Service.DurationNote = viewService.DurationNote;

                if (viewService.BrowserImage != null)
                {
                    var imageName = await UploadImage(viewService.BrowserImage);
                    Service.Image = imageName;
                }
                else
                {
                    Service.Image = null;   
                }

                _context.Services.Add(Service);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new {categoryId = categoryId});
            }

            return View(viewService);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ServiceModel? service = await _context.Services.FindAsync(id);
            ServiceViewModel serviceView = new ServiceViewModel();
            List<CategoryModel> categories = new List<CategoryModel>();
            categories = await _context.Category.ToListAsync();
            if (service == null)
            {
                return NotFound();
            }
            serviceView.CategoryOptions = new SelectList(categories, "CategoryId", "CategoryName");

            serviceView.ServiceId = service.ServiceId;
            serviceView.CategoryId = service.CategoryId;
            serviceView.ServiceName = service.ServiceName;
            serviceView.Description = service.Description;
            serviceView.Price = service.Price;
            serviceView.RequiredSiteVisit = service.RequiredSiteVisit;
            serviceView.DurationNote = service.DurationNote;
            serviceView.ExistingImage = service.Image;
            serviceView.DurationView = MinutesToDuration(service.EstimatedDurationMinutes);

            return View(serviceView);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ServiceViewModel serviceView)
        {
            List<CategoryModel> categories = await _context.Category.ToListAsync();
            serviceView.CategoryOptions = new SelectList(categories, "CategoryId", "CategoryName");
            if (ModelState.IsValid)
            {
                ServiceModel? service = await _context.Services.FindAsync(serviceView.ServiceId);
                CategoryModel? category = await _context.Category.FindAsync(serviceView.CategoryId);

                string serviceName = serviceView.ServiceName.Trim().ToLower();
                bool serviceCheck = await _context.Services.AnyAsync(x => x.ServiceName.ToLower() == serviceName && x.ServiceId != serviceView.ServiceId && x.CategoryId == serviceView.CategoryId);
                if (service == null)    
                {
                    return NotFound();
                }
                if (category == null)
                {
                    ModelState.AddModelError(nameof(serviceView.CategoryId), "Please Select a Valid Category");
                    return View(serviceView);
                }
                if(serviceCheck)
                {
                    ModelState.AddModelError(nameof(serviceView.ServiceName), "Service Name Already Present");
                    return View(serviceView);
                }
                int categoryid = category.CategoryId;
                service.CategoryId = serviceView.CategoryId;
                service.ServiceId = serviceView.ServiceId;
                service.ServiceName = serviceView.ServiceName;
                service.Description = serviceView.Description;
                service.Price = serviceView.Price;
                service.EstimatedDurationMinutes = DurationToMinutes(serviceView.DurationView.Days, serviceView.DurationView.Hours, serviceView.DurationView.Minutes);
                service.RequiredSiteVisit = serviceView.RequiredSiteVisit;
                service.DurationNote = serviceView.DurationNote;

                    if(serviceView.BrowserImage != null)
                    {
                        string imgName = await UploadImage(serviceView.BrowserImage);
                        
                        if (!string.IsNullOrEmpty(imgName))
                        {
                        service.Image = imgName;
                        DeleteImage(serviceView.ExistingImage);
                        }
                    }
                else
                {
                    service.Image = serviceView.ExistingImage;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new {categoryId = categoryid});
            }
            return View(serviceView);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceModel? service = await _context.Services.FindAsync(id);
      
            if(service == null)
            {
                return NotFound();
            }
            int categoryId = service.CategoryId;

            bool hasProviderLinks = await _context.ProviderServices.AnyAsync(x=> x.ServiceId == id);
            bool hasBooking = await _context.Bookings.AnyAsync(x => x.ServiceId == id);

            if(hasProviderLinks || hasBooking)
            {
                string message = "This service cannot be deleted. It is linked to a provider or booking";

                if (Request.Headers["X-Requested-With"]== "XMLHttpRequest")
                {
                    return Conflict(new
                    {
                        success = false,
                        message
                    });
                }

                TempData["ErrorMessage"] = message;

                return RedirectToAction(nameof(Index), new { categoryId = categoryId });    
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            DeleteImage(service.Image);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true ,serviceId = id});
            }

            return RedirectToAction("Index", new {categoryId = categoryId});
        }
         
        private int DurationToMinutes(int Days,int Hours, int Minutes)
        {
            int DurationInMinutes = ( Days*1440)+(Hours*60)+(Minutes*1);
            return DurationInMinutes;
        }
        private DurationViewModel MinutesToDuration(int? time)
        {
            DurationViewModel Duration = new DurationViewModel();

            if (time == null)
            {
                return Duration;
            }

            int totalMinutes = time.Value;

            int Days = totalMinutes / 1440;
            int remainingTime = totalMinutes % 1440;

            int Hours = remainingTime / 60;
            int Minutes = remainingTime % 60;

            Duration.Days = Days;
            Duration.Hours = Hours;
            Duration.Minutes = Minutes;

            return Duration ;
        } 
        private async Task<string> UploadImage(IFormFile BrowserImage)
        {
            string fileextexsion = Path.GetExtension(BrowserImage.FileName) ;
            string fileName = Guid.NewGuid().ToString() + fileextexsion;
            var folderPath = Path.Combine(_environment.WebRootPath, "images");
            var fileUploadPath = Path.Combine(folderPath, fileName);

            using(var stream = new FileStream(fileUploadPath, FileMode.Create))
            {
                await BrowserImage.CopyToAsync(stream);
            }
            return fileName;
        } 
        private void DeleteImage(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var folderPath = Path.Combine(_environment.WebRootPath, "images");
            var fileUploadPath = Path.Combine(folderPath, fileName);

            if (System.IO.File.Exists(fileUploadPath))
            {
                System.IO.File.Delete(fileUploadPath);
            }
        }

    }
}
