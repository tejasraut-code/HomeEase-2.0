using System.Diagnostics;
using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeEase_2._0_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;  //EF DI rough meaning

        private readonly IWebHostEnvironment _environment; //for receiving img from user and upload on localstorage.

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {

            List<CategoryModel> categories = _context.Category.ToList();

            return View(categories);
        }

        //Create 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel viewCategory)
        {
            if (ModelState.IsValid)
            {
                CategoryModel category = new CategoryModel();

                if(viewCategory.BrowserImage != null)
                {
                    //var fileName = viewCategory.BrowserImage.FileName;
                    //var filePath = Path.Combine(_environment.WebRootPath, "images");
                    //var fileUploads = Path.Combine(filePath, fileName);

                    //using(var stream = new FileStream(fileUploads, FileMode.Create))
                    //{
                    //    viewCategory.BrowserImage.CopyTo(stream);
                    //};

                    var fileName =  UploadImage(viewCategory.BrowserImage);

                    category.Image = fileName;
                }
                else
                {
                    category.Image = "default.jpg";
                }

                category.CategoryName = viewCategory.CategoryName;
                category.Description = viewCategory.Description;
                _context.Category.Add(category);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(viewCategory);
        }

        //Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            CategoryModel? category = _context.Category.Find(id);

            if(category == null)
            {
                return NotFound();
            }

            CategoryViewModel viewCategory = new CategoryViewModel();

            viewCategory.CategoryId = category.CategoryId;
            viewCategory.CategoryName = category.CategoryName;
            viewCategory.Description = category.Description;
            viewCategory.ExistingImage = category.Image;

            return View(viewCategory);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel viewCategory)
        {
            if (ModelState.IsValid)
            {
                string? existingImgPath = null;
                string? fileName = null;

                CategoryModel? category =  _context.Category.Find(viewCategory.CategoryId);
                if(category == null)
                {
                    return NotFound();
                }

                if(viewCategory.BrowserImage != null)
                {
                    fileName = UploadImage(viewCategory.BrowserImage);
                    category.Image = fileName;
                    existingImgPath = Path.Combine(_environment.WebRootPath, "images", viewCategory.ExistingImage);
                }
                else
                {
                    category.Image = viewCategory.ExistingImage;
                }
                category.CategoryName = viewCategory.CategoryName;
                category.Description = viewCategory.Description;

                _context.Category.Update(category);
                _context.SaveChanges();

                if (fileName != null && existingImgPath!=null && System.IO.File.Exists(existingImgPath) && viewCategory.ExistingImage != fileName)
                {
                    DeleteImage(viewCategory.ExistingImage);
                }

                return RedirectToAction("Index");

            }
            return View(viewCategory);
        }


        //Delete 
        [HttpPost]
        public IActionResult Delete(int id)
        {
            CategoryModel? category = _context.Category.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            _context.SaveChanges();

            DeleteImage(category.Image);

            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        private string UploadImage(IFormFile browserImage)
        {
            var fileName = browserImage.FileName;
            var folderPath = Path.Combine(_environment.WebRootPath, "images");
            var fileUploadPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fileUploadPath, FileMode.Create))
            {
                browserImage.CopyTo(stream);
            }
            return fileName;
        }

        private void DeleteImage(string imageName)
        {
            var folderPath = Path.Combine(_environment.WebRootPath, "images");
            var fileDeletePath = Path.Combine(folderPath, imageName);

            if (System.IO.File.Exists(fileDeletePath))
            {
                System.IO.File.Delete(fileDeletePath);
            }
        }
    }
}
