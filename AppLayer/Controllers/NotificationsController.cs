using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SmartClinic.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return View(notifications);
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> MyNotifications()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var notifications = await _notificationService.GetPatientNotificationsAsync(email);
            return View(notifications);
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorNotifications()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var notifications = await _notificationService.GetDoctorNotificationsAsync(email);
            return View(notifications);
        }
    }
}