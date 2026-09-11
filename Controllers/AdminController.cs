using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Booking()
        {
            string? userRole = HttpContext.Session.GetString("Role");
            if(userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            List<BookingAdminViewModel> bookingAdminView = new List<BookingAdminViewModel>();
            List<BookingModel> bookings = _context.Bookings.Include(x =>x.User).OrderByDescending(x => x.CreatedAt).ToList();
                foreach (var item in bookings)
                {
                    BookingAdminViewModel adminViewModel = new BookingAdminViewModel();
                    adminViewModel.BookingId = item.BookingId;
                    adminViewModel.CustomerName = item.User?.UserName ?? "Unknown User";
                    adminViewModel.ServiceName = item.ServiceNameAtBooking;
                    adminViewModel.Price = item.PriceAtBooking;
                    adminViewModel.DurationView = MinutesToDuration(item.DurationMinutesAtBooking);
                    adminViewModel.CreatedAt = item.CreatedAt;
                    adminViewModel.ScheduledFor = item.ScheduledFor;
                    adminViewModel.ServiceAddress = item.ServiceAddress;
                    adminViewModel.CustomerNote = item.CustomerNote;
                    adminViewModel.BookingStatus = item.BookingStatus;

                    bookingAdminView.Add(adminViewModel);
                }
             return View(bookingAdminView);
        }

        //[HttpPost]
        //public IActionResult Confirm(int bookingId)
        //{
        //    string? userRole = HttpContext.Session.GetString("Role");
        //    if(userRole != "Admin")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    BookingModel? booking = _context.Bookings.FirstOrDefault(x=>x.BookingId == bookingId);
        //    if(booking == null)
        //    {
        //        return NotFound();
        //    }

        //    if(booking.BookingStatus == "Pending")
        //    {
        //        booking.BookingStatus = "Confirmed";
        //        _context.SaveChanges();

        //        return RedirectToAction("Booking", "Admin");
        //    }
        //    return RedirectToAction("Booking","Admin");
        //}

        [HttpPost]
        public IActionResult Complete(int bookingId)
        {
            string? userRole = HttpContext.Session.GetString("Role");
            if(userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            BookingModel? booking = _context.Bookings.FirstOrDefault(x => x.BookingId == bookingId);
            if(booking == null)
            {
                return NotFound();
            }
            if(booking.BookingStatus == "Confirmed")
            {
                booking.BookingStatus = "Completed";

                _context.SaveChanges();
            }
            return RedirectToAction("Booking", "Admin");
        }

        [HttpGet]
        public IActionResult Provider()
        {
            string? userRole = HttpContext.Session.GetString("Role");

            if(userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            List<ProviderProfileModel> providerProfiles = _context.ProviderProfiles.Include(x => x.User).ToList();
    
            List<ProviderAdminViewModel> providerAdminViewsList = new List<ProviderAdminViewModel>();

                foreach(var item in providerProfiles)
                {
                    ProviderAdminViewModel providerAdminView = new ProviderAdminViewModel();

                    providerAdminView.ProviderId = item.ProviderId;
                    providerAdminView.UserName = item.User?.UserName??"Unknown User";
                    providerAdminView.Email = item.User?.Email?? string.Empty;
                    providerAdminView.Mobile = item.User?.Mobile?? string.Empty;
                    providerAdminView.ExperienceYears = item.ExperienceYears;
                    providerAdminView.Bio = item.Bio;
                    providerAdminView.ServiceArea = item.ServiceArea;
                    providerAdminView.IsApproved = item.IsApproved;

                    providerAdminViewsList.Add(providerAdminView);
                }

            return View(providerAdminViewsList);
        }

        [HttpPost]
        public IActionResult ApproveProvider(int providerId)
        {
            string? userRole = HttpContext.Session.GetString("Role");
            if(userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ProviderProfileModel? providerProfile = _context.ProviderProfiles.Find(providerId);
            if(providerProfile == null)
            {
                return NotFound();
            }

            if (!providerProfile.IsApproved)
            {
                providerProfile.IsApproved = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Provider");
        }

        [HttpGet]
        public IActionResult AssignProvider(int bookingId)
        {
            if(!CheckAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            AssignProviderViewModel AssignProviderView = new AssignProviderViewModel();
            BookingModel? Booking = _context.Bookings.Find(bookingId);
            if(Booking == null)
            {
                return NotFound();
            }

            List<ProviderServiceModel> ProviderService = _context.ProviderServices.Include( x=> x.ProviderProfile).ThenInclude( x => x!.User).Where( x=>x.ServiceId == Booking.ServiceId && x.ProviderProfile != null && x.ProviderProfile.IsApproved).ToList();

            if (!ProviderService.Any())
            {
                ViewBag.Message = "No Approved Provider is Available for this Service.";
            }

            AssignProviderView.BookingId = bookingId;
            AssignProviderView.ServiceName = Booking.ServiceNameAtBooking;
            foreach( var item in ProviderService)
            {
                AssignProviderView.ProviderOptions.Add(new SelectListItem
                {
                    Text = item.ProviderProfile?.User?.UserName?? "Unknown User",
                    Value = item.ProviderId.ToString()
                });
            }

            return View(AssignProviderView);
        }
        [HttpPost]
        public IActionResult AssignProvider(AssignProviderViewModel assignProviderView)
        {
            if (!CheckAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            BookingModel? booking = _context.Bookings.Find(assignProviderView.BookingId);
            if (booking == null)
            {
                return NotFound();
            }   
            if(booking.BookingStatus != "Pending")
            {
                return RedirectToAction("Booking", "Admin");
            }
            assignProviderView.ServiceName = booking.ServiceNameAtBooking;

            bool alreadyAssigned = _context.BookingProviders.Any(x => x.BookingId == booking.BookingId);
            if (alreadyAssigned)
            {
                return RedirectToAction("Booking", "Admin");
            }

            List<ProviderServiceModel> providerService = _context.ProviderServices.Include(x => x.ProviderProfile).ThenInclude(x => x!.User).Where(x => x.ServiceId == booking.ServiceId && x.ProviderProfile != null && x.ProviderProfile.IsApproved).ToList();

            if (!providerService.Any())
            {
                ViewBag.Message = "No Approved provider is available for this service";
            }

             foreach( var item in providerService)
            {
                assignProviderView.ProviderOptions.Add(new SelectListItem{
                    Text = item.ProviderProfile?.User?.UserName?? "Unknown User",
                    Value = item.ProviderId.ToString()
                });
            }

            bool validProvider = providerService.Any(x => x.ProviderId == assignProviderView.SelectedProviderId);
            if (!validProvider)
            {
                ModelState.AddModelError(nameof(assignProviderView.SelectedProviderId), "Select a Valid Provider");
            }

            if (ModelState.IsValid)
            {
                BookingProviderModel bookingProvider = new BookingProviderModel();

                bookingProvider.ProviderId = assignProviderView.SelectedProviderId;
                bookingProvider.BookingId = assignProviderView.BookingId;

                if(booking.BookingStatus == "Pending")
                {
                    booking.BookingStatus = "Confirmed";
                }

                _context.BookingProviders.Add(bookingProvider);
                _context.SaveChanges();

                return RedirectToAction("Booking", "Admin");
            }
            
            return View(assignProviderView);
        }

        private DurationViewModel MinutesToDuration(int? minutes)
        {
            DurationViewModel duration = new DurationViewModel();
            if(minutes == null)
            {
                return duration;
            }
            int totalTime = minutes.Value;
            duration.Days = totalTime / 1440;
            int remainingTime = totalTime % 1440;

            duration.Hours = remainingTime / 60;
            duration.Minutes = remainingTime % 60;

            return duration;
        }

        private bool CheckAdmin()
        {
            string? Role = HttpContext.Session.GetString("Role");
            if (Role == "Admin")
            {
                return true;
            }
            return false;
        }
    }
}
