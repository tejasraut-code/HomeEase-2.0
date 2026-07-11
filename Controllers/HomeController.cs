using System.Diagnostics;
using HomeEase_2._0_MVC.Models;
using HomeEase_2._0_MVC.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeEase_2._0_MVC.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<CategoriesModels> categories = new()
        {
            new CategoriesModels
            {
                CategoriesId = 1,
                CategoriesName="Plumber",
                Description="Expert Plumbing services for home and offices.",
                Images="Plumber.jpg",
            },

            new CategoriesModels
            {
                CategoriesId=2,
                CategoriesName="Cleaning",
                Description="Home and Office Cleaning",
                Images="Cleaner.jpg",
            },
            new CategoriesModels
            {
                CategoriesId = 3,
                CategoriesName="Electrician",
                Description="Safe Electricity Repairs",
                Images="Electrician.jpg",
            },

            new CategoriesModels
            {
                CategoriesId=4,
                CategoriesName="Painting",
                Description="Interior and Exterior Painting",
                Images="Painter.jpg",
            },
            new CategoriesModels
            {
                CategoriesId=5,
                CategoriesName="Carpenter",
                Description="furntiure repair &  woodwork",
                Images="Carpenter.jpg"
            }
        };

            return View(categories);
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
