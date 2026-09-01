using HomeEase_2._0_MVC.Data;
using HomeEase_2._0_MVC.Models.DomainModels;
using HomeEase_2._0_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        //private readonly IWebHostEnvironment _environment;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel registerView)
        {
            UserModel user = new UserModel();
            PasswordHasher<UserModel> passwordHasher = new PasswordHasher<UserModel>();
            if (ModelState.IsValid)
            {
                string EmailId = registerView.Email.Trim().ToLower();
                var usercheck = _context.Users.Any(x => x.Email.ToLower() == EmailId);
                if (usercheck)
                {
                    ModelState.AddModelError(nameof(registerView.Email) ,errorMessage: "EmailId Already registered. Try to Register with New EmailId");
                    return View(registerView);
                }

                user.UserName = registerView.UserName.Trim();
                user.Email = EmailId;
                user.Mobile = registerView.Mobile;
                //user.PasswordHash = registerView.Password.Trim();
                user.PasswordHash = passwordHasher.HashPassword(user, registerView.Password);
                user.Address = registerView.Address.Trim();
                user.City = registerView.City.Trim();
                user.Role = "Customer";

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(registerView);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
