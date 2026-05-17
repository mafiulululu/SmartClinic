using AppLayer.Models;
using BLL.Services;
using DAL.EF.Table;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.ViewModels;
using System.Security.Claims;

namespace SmartClinic.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.LoginUserAsync(
                model.Email,
                model.Password,
                model.Role
            );

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email, password, or role.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "AdminDashboard");
            }

            if (user.Role == "Doctor")
            {
                return RedirectToAction("Index", "DoctorDashboard");
            }

            if (user.Role == "Patient")
            {
                return RedirectToAction("Index", "PatientDashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = model.Password
            };

            var result = await _authService.RegisterUserAsync(
                newUser,
                model.Phone,
                model.Dob,
                model.Speciality,
                model.ConsultationFee,
                model.IsAvailable
            );

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}