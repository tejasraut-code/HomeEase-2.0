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
                CategoryModel? category =  _context.Category.Find(viewCategory.CategoryId);
                if(category == null)
                {
                    return NotFound();
                }

                if(viewCategory.BrowserImage != null)
                {
                    var fileName = viewCategory.BrowserImage.FileName;
                    var filePath = Path.Combine(_environment.WebRootPath , "images");
                    var fileUpload = Path.Combine(filePath,fileName);

                    using(var stream = new FileStream( fileUpload, FileMode.Create))
                    {
                        viewCategory.BrowserImage.CopyTo(stream);
                    }

                    category.Image = fileName;

                    //var ExitsingImgPath = Path.Combine(filePath, viewCategory.ExistingImage);

                }
                else
                {
                    category.Image = viewCategory.ExistingImage;
                }
                category.CategoryName = viewCategory.CategoryName;
                category.Description = viewCategory.Description;

                _context.Category.Update(category);
                _context.SaveChanges();

                var ExitsingImgPath = Path.Combine(_environment.WebRootPath, "images", viewCategory.ExistingImage);

                if (System.IO.File.Exists(ExitsingImgPath))
                {
                    System.IO.File.Delete(ExitsingImgPath);
                }

            }
            return View(viewCategory);
        }


        //Delete 
        [HttpPost]
        public IActionResult Delete(int id)
        {
            CategoryModel delCategory = _context.Category.Find(id);

            if(delCategory == null)
            {
                return NotFound();
            }

            _context.Category.Remove(delCategory);
            _context.SaveChanges();

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
