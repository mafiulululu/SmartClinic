using BLL.Services;
using DAL.EF.Table;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.ViewModels;

namespace SmartClinic.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        // Inject the BLL service
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Account/Login (Loads the blank login page)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login (Processes the login attempt)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // In a real app, you would validate against the database here.
                // For now, we simulate a successful login and handle the routing:
                switch (model.Role)
                {
                    case "Admin":
                        // Redirects to an AdminDashboard controller
                        return RedirectToAction("Index", "AdminDashboard");

                    case "Doctor":
                        // Redirects to the Doctors controller you already built!
                        return RedirectToAction("Index", "Doctors");

                    case "Patient":
                        // Redirects to a PatientDashboard controller
                        return RedirectToAction("Index", "PatientDashboard");

                    default:
                        ModelState.AddModelError("", "Invalid role selected.");
                        break;
                }
            }

            // If validation fails, reload the page with the error messages
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the ViewModel data to the actual Database Model
                var newUser = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Role = model.Role,
                    // Note: In a production app, you MUST encrypt this password using BCrypt or Identity!
                    // We are saving it directly here to keep the architecture simple for your project.
                    PasswordHash = model.Password,
                    CreatedAt = DateTime.Now
                };

                // Send to the BLL
                bool isSuccess = await _authService.RegisterUserAsync(newUser);

                if (isSuccess)
                {
                    // If successful, send them to the login page!
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                }
            }

            // If validation fails, reload the form with errors
            return View(model);
        }
    }
}
