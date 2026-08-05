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

      
        public IActionResult Index()
        {

            List<CategoryModel> categories = _context.Category.ToList();

            //    List<CategoriesModels> categories = new()
            //{
            //    new CategoriesModels
            //    {
            //        CategoriesId = 1,
            //        CategoriesName="Plumber",
            //        Description="Expert Plumbing services for home and offices.",
            //        Images="Plumber.jpg",
            //    },

            //    new CategoriesModels
            //    {
            //        CategoriesId=2,
            //        CategoriesName="Cleaning",
            //        Description="Home and Office Cleaning",
            //        Images="Cleaner.jpg",
            //    },
            //    new CategoriesModels
            //    {
            //        CategoriesId = 3,
            //        CategoriesName="Electrician",
            //        Description="Safe Electricity Repairs",
            //        Images="Electrician.jpg",
            //    },

            //    new CategoriesModels
            //    {
            //        CategoriesId=4,
            //        CategoriesName="Painting",
            //        Description="Interior and Exterior Painting",
            //        Images="Painter.jpg",
            //    },
            //    new CategoriesModels
            //    {
            //        CategoriesId=5,
            //        CategoriesName="Carpenter",
            //        Description="furntiure repair &  woodwork",
            //        Images="Carpenter.jpg"
            //    }
            //};

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
                    var fileName = viewCategory.BrowserImage.FileName;
                    var filePath = Path.Combine(_environment.WebRootPath, "images");
                    var fileUploads = Path.Combine(filePath, fileName);

                    using(var stream = new FileStream(fileUploads, FileMode.Create))
                    {
                        viewCategory.BrowserImage.CopyTo(stream);
                    };

                    category.CategoryName = viewCategory.CategoryName;
                    category.Description = viewCategory.Description;
                    category.Image = fileName;
                }
                else
                {
                    category.CategoryName = viewCategory.CategoryName;
                    category.Description = viewCategory.Description;
                    category.Image = "default.jpg";
                }

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
                    fileName = viewCategory.BrowserImage.FileName;
                    var filePath = Path.Combine(_environment.WebRootPath , "images");
                    var fileUpload = Path.Combine(filePath,fileName);

                    existingImgPath = Path.Combine(filePath, viewCategory.ExistingImage);

                    using (var stream = new FileStream( fileUpload, FileMode.Create))
                    {
                        viewCategory.BrowserImage.CopyTo(stream);
                    }

                    category.Image = fileName;
                }
                else
                {
                    category.Image = viewCategory.ExistingImage;
                }
                category.CategoryName = viewCategory.CategoryName;
                category.Description = viewCategory.Description;

                _context.Category.Update(category);
                _context.SaveChanges();

                //var ExitsingImgPath = Path.Combine(_environment.WebRootPath, "images", viewCategory.ExistingImage);

                if (fileName != null && existingImgPath!=null && System.IO.File.Exists(existingImgPath) && viewCategory.ExistingImage != fileName)
                {
                    System.IO.File.Delete(existingImgPath);
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
            var filePath = Path.Combine(_environment.WebRootPath, "images");
            var fileDeletePath = Path.Combine(filePath, category.Image);

            _context.Category.Remove(category);
            _context.SaveChanges();

            if (System.IO.File.Exists(fileDeletePath))
            {
                System.IO.File.Delete(fileDeletePath);
            }
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
    }
}
