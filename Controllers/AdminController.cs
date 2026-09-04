using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            List<BookingModel> bookings = _context.Bookings.Include(x =>x.User).ToList();
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

        [HttpPost]
        public IActionResult Confirm(int bookingId)
        {
            string? userRole = HttpContext.Session.GetString("Role");
            if(userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            BookingModel? booking = _context.Bookings.FirstOrDefault(x=>x.BookingId == bookingId);
            if(booking == null)
            {
                return NotFound();
            }

            if(booking.BookingStatus == "Pending")
            {
                booking.BookingStatus = "Confirmed";
                _context.SaveChanges();

                return RedirectToAction("Booking", "Admin");
            }
            return RedirectToAction("Booking","Admin");
        }

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
                return RedirectToAction("Booking", "Admin");
            }
            return RedirectToAction("Booking", "Admin");
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
    }
}
