using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Controllers
{
    public class ProviderController : Controller
    {
        private readonly AppDbContext _context;

        public ProviderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            string? userRole = HttpContext.Session.GetString("Role");
            if(userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if(userRole != "ServiceProvider")
            {
                return RedirectToAction("Index", "Home");
            }

            ProviderProfileModel? providerProfile = _context.ProviderProfiles.Include(x => x.User).FirstOrDefault(x => x.UserId == userId);
            if(providerProfile == null)
            {
                return NotFound();
            }


            List<ProviderServiceModel>providerServices = _context.ProviderServices.Include( x=> x.Service).Where(x => x.ProviderId == providerProfile.ProviderId).ToList();

            ProviderDashboardViewModel providerDashboardView = new ProviderDashboardViewModel();

            providerDashboardView.ProviderId = providerProfile.ProviderId;
            providerDashboardView.ProviderName = providerProfile.User?.UserName ?? "Unknown User";
            providerDashboardView.ExperienceYears = providerProfile.ExperienceYears;
            providerDashboardView.ServiceArea = providerProfile.ServiceArea;
            providerDashboardView.Bio = providerProfile.Bio;

            if (!providerProfile.IsApproved)
            {
                ViewBag.Message = "Waiting for Approval";
                return View(providerDashboardView);
            }

            foreach (var item in providerServices)
            {
                if(item.Service != null)
                {
                    providerDashboardView.ServiceNames.Add(item.Service.ServiceName);
                }
            }

            List<BookingProviderModel> bookingProvider = _context.BookingProviders.Include(x => x.Booking).ThenInclude(x => x!.User).Include(x => x.Booking).ThenInclude(x => x!.Service).ThenInclude(x => x!.Category).Where(x => x.ProviderId == providerProfile.ProviderId).ToList();

            foreach(var item in bookingProvider)
            {
                if(item.Booking == null)
                {
                    continue;
                }
                ProviderBookingViewModel providerBookingView = new ProviderBookingViewModel();
                providerBookingView.BookingId = item.BookingId;
                providerBookingView.CustomerName = item.Booking.User?.UserName?? "Unknown User";
                providerBookingView.Mobile = item.Booking.User?.Mobile?? "Not Available";
                providerBookingView.City = item.Booking.User?.City?? "Not Available";

                providerBookingView.ServiceAddress = item.Booking.ServiceAddress??"Not Available";
                providerBookingView.ServiceName = item.Booking.ServiceNameAtBooking??"Not Available";
                providerBookingView.CategoryName = item.Booking.Service?.Category?.CategoryName??"Unknown Service";

                providerBookingView.CustomerNote = item.Booking.CustomerNote;

                providerBookingView.BookedOn = item.Booking.CreatedAt;
                providerBookingView.ScheduledFor = item.Booking.ScheduledFor;

                providerBookingView.ServicePrice = item.Booking.PriceAtBooking;
                providerBookingView.BookingStatus = item.Booking.BookingStatus;

                providerDashboardView.ProviderBookingView.Add(providerBookingView);
            }

            return View(providerDashboardView);
        }

        [HttpPost]
        public IActionResult CompleteBooking(int bookingId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            string? userRole = HttpContext.Session.GetString("Role");
            if(userRole != "ServiceProvider")
            {
                return RedirectToAction("Index", "Home");
            }

            ProviderProfileModel? providerProfile = _context.ProviderProfiles.FirstOrDefault( x=> x.UserId == userId);
            if(providerProfile == null)
            {
                return RedirectToAction("Login","Account");
            }
            BookingProviderModel? bookingProvider = _context.BookingProviders.Include(x => x.Booking).FirstOrDefault(x => x.BookingId == bookingId && x.ProviderId == providerProfile.ProviderId);
            if(bookingProvider == null)
            {
                return NotFound();
            }
            if(!providerProfile.IsApproved)
            {
                return RedirectToAction("Index", "Provider");
            }

            if (bookingProvider.Booking?.BookingStatus == "Confirmed")
            {
                bookingProvider.Booking.BookingStatus = "Completed";

                _context.SaveChanges();
                return RedirectToAction("Index", "Provider");
            }

            return RedirectToAction("Index", "Provider");
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

            if(providerRegisterView.SelectedServiceIds == null  ||  !providerRegisterView.SelectedServiceIds.Any())
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
