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

        [HttpGet]
        public IActionResult Index(int? categoryId)
        {
            string categoryName = "All Services";
            //List<ServiceModel> serviceViews = _context.Services.ToList();
            //List<ServiceModel> serviceViews = _context.Services.Include(x => x.Category).ToList();

            if(categoryId != null)
            {
                CategoryModel? category = _context.Category.Find(categoryId);
                if (category != null)
                {
                    categoryName = category.CategoryName;
                }
            }
            List<ServiceModel> serviceViews = _context.Services.Where(x => categoryId == null || x.CategoryId == categoryId).ToList();
            List<ServiceIndexViewModel> serviceIndexViewsList = new List<ServiceIndexViewModel>();
           
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

                serviceIndexViewsList.Add(serviceIndexView);
            }
            ViewBag.categoryName = categoryName;
            return View(serviceIndexViewsList);
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
            List<CategoryModel> categories = _context.Category.ToList();
            viewService.CategoryOptions = new SelectList(categories, "CategoryId", "CategoryName");

            if (ModelState.IsValid)
            {
                ServiceModel Service = new ServiceModel();
                string servicename = viewService.ServiceName.Trim().ToLower();

                int categoryId = viewService.CategoryId;
                CategoryModel? category = _context.Category.Find(categoryId);

                var serviceNamePresent = _context.Services.Any( x => x.ServiceName.ToLower() == servicename);

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
                    var imageName = UploadImage(viewService.BrowserImage);
                    Service.Image = imageName;
                }
                else
                {
                    Service.Image = null;
                }

                _context.Services.Add(Service);
                _context.SaveChanges();

                return RedirectToAction("Index", new {categoryId = categoryId});
            }

            return View(viewService);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ServiceModel? service = _context.Services.Find(id);
            ServiceViewModel serviceView = new ServiceViewModel();
            List<CategoryModel> categories = new List<CategoryModel>();
            categories = _context.Category.ToList();
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
        public IActionResult Edit(ServiceViewModel serviceView)
        {
            List<CategoryModel> categories = _context.Category.ToList();
            serviceView.CategoryOptions = new SelectList(categories, "CategoryId", "CategoryName");
            if (ModelState.IsValid)
            {
                ServiceModel? service = _context.Services.Find(serviceView.ServiceId);
                CategoryModel? category = _context.Category.Find(serviceView.CategoryId);

                string serviceName = serviceView.ServiceName.Trim().ToLower();
                bool serviceCheck = _context.Services.Any(x => x.ServiceName.ToLower() == serviceName && x.ServiceId != serviceView.ServiceId && x.CategoryId == serviceView.CategoryId);
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
                        string imgName =  UploadImage(serviceView.BrowserImage);
                        
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

                //_context.Services.Update(service);
                _context.SaveChanges();

                return RedirectToAction("Index", new {categoryid = categoryid});
            }
            return View(serviceView);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            ServiceModel? service = _context.Services.Find(id);
      
            if(service == null)
            {
                return NotFound();
            }
            int categoryId = service.CategoryId;
            _context.Services.Remove(service);
            _context.SaveChanges();

            DeleteImage(service.Image);
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
        private string UploadImage(IFormFile BrowserImage)
        {
            string fileextexsion = Path.GetExtension(BrowserImage.FileName) ;
            string fileName = Guid.NewGuid().ToString() + fileextexsion;
            var folderPath = Path.Combine(_environment.WebRootPath, "images");
            var fileUploadPath = Path.Combine(folderPath, fileName);

            using(var stream = new FileStream(fileUploadPath, FileMode.Create))
            {
                BrowserImage.CopyTo(stream);
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
