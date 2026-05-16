using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartClinic.Web.ViewModels;
using System.Security.Claims;

namespace SmartClinic.Web.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IDoctorService doctorService
        )
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
        }

        // Admin: view all appointments
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        // Patient: view own appointments
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> MyAppointments()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = await _appointmentService.GetPatientAppointmentsAsync(email);
            return View(appointments);
        }

        // Doctor: view own appointments
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorAppointments()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(email);
            return View(appointments);
        }

        // Patient: open booking form
        [Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDoctorsDropdown();
            return View();
        }

        // Patient: submit booking form
        [Authorize(Roles = "Patient")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDoctorsDropdown();
                return View(model);
            }

            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _appointmentService.BookAppointmentAsync(
                email,
                model.DoctorId,
                model.AppointmentDate,
                model.SymptomsNotes
            );

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                await LoadDoctorsDropdown();
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(MyAppointments));
        }

        private async Task LoadDoctorsDropdown()
        {
            var doctors = await _doctorService.GetDoctorsListAsync();

            var availableDoctors = doctors
                .Where(d => d.IsAvailable)
                .Select(d => new
                {
                    d.DoctorId,
                    DisplayName = $"Dr. {d.FirstName} {d.LastName} - {d.Speciality} - Fee: {d.ConsultationFee:0.00}"
                })
                .ToList();

            ViewBag.Doctors = new SelectList(
                availableDoctors,
                "DoctorId",
                "DisplayName"
            );
        }
    }
}