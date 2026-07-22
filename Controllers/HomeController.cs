using System.Diagnostics;
using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models;
using HomeEase_2._0_MVC.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeEase_2._0_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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
        public IActionResult Create(CategoryModel category)
        {
            if(ModelState.IsValid)
            {
                _context.Category.Add(category);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        //Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {

            CategoryModel? categoryedit = _context.Category.Find(id);

            if(categoryedit == null)
            {
                return NotFound();
            }

            return View(categoryedit);
        }

        [HttpPost]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Update(category);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
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
