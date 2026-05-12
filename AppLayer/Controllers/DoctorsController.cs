using BLL.Services;
using DAL.EF.Table;
using Microsoft.AspNetCore.Mvc;

namespace SmartClinic.Web.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: /Doctors/
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetDoctorsListAsync();
            return View(doctors);
        }

        // GET: /Doctors/Create (Loads the blank form)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Doctors/Create (Catches the submitted form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            // Remove validation for auto-generated and navigation properties
            ModelState.Remove("DoctorId");
            ModelState.Remove("Appointments");
            ModelState.Remove("Notifications");

            if (ModelState.IsValid)
            {
                await _doctorService.CreateDoctorAsync(doctor);
                return RedirectToAction(nameof(Index));
            }

            return View(doctor);
        }
    }
}