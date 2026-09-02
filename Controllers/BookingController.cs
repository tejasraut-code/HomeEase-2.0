using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HomeEase_2._0_MVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int serviceId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            BookingViewModel bookingView = new BookingViewModel();

            ServiceModel? service = _context.Services.FirstOrDefault(x => x.ServiceId == serviceId);
            if(service == null)
            {
                return NotFound();
            }

            bookingView.ServiceId = service.ServiceId;
            bookingView.ServiceName = service.ServiceName;
            bookingView.Price = service.Price;
            bookingView.DurationView = MinutesToDuration(service.EstimatedDurationMinutes);
            bookingView.ScheduledFor = DateTime.Now.AddDays(1);

            return View(bookingView);
        }
        [HttpPost]
        public IActionResult Create(BookingViewModel bookingView)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            BookingModel booking = new BookingModel();

            ServiceModel? service = _context.Services.Find(bookingView.ServiceId);
            if (service == null)
            {
                ModelState.AddModelError("", "Service No longer Available");
                return NotFound();
            }
            if(bookingView.ScheduledFor <= DateTime.Now)
            {
                ModelState.AddModelError("", "Select a Future Date and Time");
            }
            if (ModelState.IsValid)
            {
                booking.UserId = userId.Value;
                booking.ServiceId = service.ServiceId;
                booking.ServiceNameAtBooking = service.ServiceName;
                booking.PriceAtBooking = service.Price;
                booking.DurationMinutesAtBooking = service.EstimatedDurationMinutes;
                booking.ScheduledFor = bookingView.ScheduledFor;
                booking.ServiceAddress = bookingView.ServiceAddress;
                booking.CustomerNote = bookingView.CustomerNote;

                _context.Bookings.Add(booking);
                _context.SaveChanges();

                return RedirectToAction("Index","Home");
            }
            else
            {
                bookingView.ServiceId = service.ServiceId;
                bookingView.ServiceName = service.ServiceName;
                bookingView.Price = service.Price;
                bookingView.DurationView = MinutesToDuration(service.EstimatedDurationMinutes);
            }
                return View(bookingView);
        }

        private DurationViewModel MinutesToDuration(int? duration)
        {
            DurationViewModel durationView = new DurationViewModel();
            if(duration == null)
            {
                return durationView;
            }

            int Time = duration.Value;

            int day = Time / 1440;
            int remainingTime = Time % 1440;

            int hours = remainingTime / 60;
            int minutes = remainingTime % 60;

            durationView.Days = day;
            durationView.Hours = hours;
            durationView.Minutes = minutes;

            return durationView;

        }

    }
}
