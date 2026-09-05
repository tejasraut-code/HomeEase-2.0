using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeEase_2._0_MVC.Controllers
{
    public class ProviderController : Controller
    {
        private readonly AppDbContext _context;

        public ProviderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ProviderRegisterViewModel providerRegisterView = new ProviderRegisterViewModel();

            providerRegisterView.ServiceOptions = _context.Services.Select(x => new SelectListItem
            {
                Value = x.ServiceId.ToString(),
                Text = x.ServiceName
            })
                .ToList();

            return View(providerRegisterView);

        }
    }
}
