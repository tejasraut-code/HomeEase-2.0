using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
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
        [HttpPost]
        public IActionResult Register(ProviderRegisterViewModel providerRegisterView)
        {
            providerRegisterView.ServiceOptions = _context.Services.Select(x => new SelectListItem
            {
                Text = x.ServiceName,
                Value = x.ServiceId.ToString()
            }).ToList();

            if(!providerRegisterView.SelectedServiceIds.Any() || providerRegisterView.SelectedServiceIds == null)
            {
                ModelState.AddModelError(nameof(providerRegisterView.SelectedServiceIds), "Please select at least one service.");
                return View(providerRegisterView);
            }

            if (ModelState.IsValid)
            {
                string emailcheck = providerRegisterView.EmailId.Trim().ToLower();
                UserModel? user1 = _context.Users.FirstOrDefault(x => x.Email.ToLower() == emailcheck);
                if(user1 != null)
                {
                    ModelState.AddModelError(nameof(providerRegisterView.EmailId), "Email Id Already Registered, Use new EmailId");
                    return View(providerRegisterView);
                }

                ProviderProfileModel providerProfile = new ProviderProfileModel();
                PasswordHasher<UserModel> passwordHasher = new PasswordHasher<UserModel>();
                UserModel user = new UserModel();

                user.UserName = providerRegisterView.UserName.Trim();
                user.Email = emailcheck;
                user.Mobile = providerRegisterView.Mobile;
                user.Address = providerRegisterView.Address.Trim();
                user.City = providerRegisterView.City.Trim();
                user.Role = "ServiceProvider";
                //user.CreatedOn = DateTime.Now;
                user.PasswordHash = passwordHasher.HashPassword(user, providerRegisterView.Password);

                providerProfile.ExperienceYears = providerRegisterView.ExperienceYears;
                providerProfile.Bio = providerRegisterView.Bio;
                providerProfile.ServiceArea = providerRegisterView.ServiceArea.Trim();
                //providerProfile.CreatedAt = DateTime.Now;
                providerProfile.IsApproved = false;
                providerProfile.User = user;

                _context.Users.Add(user);
                _context.ProviderProfiles.Add(providerProfile);

                foreach(var serviceId in providerRegisterView.SelectedServiceIds)
                {
                    ProviderServiceModel providerService = new ProviderServiceModel();
                    providerService.ProviderProfile = providerProfile;
                    providerService.ServiceId = serviceId;
                    _context.ProviderServices.Add(providerService);
                }

                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(providerRegisterView);
        }
    }
}
