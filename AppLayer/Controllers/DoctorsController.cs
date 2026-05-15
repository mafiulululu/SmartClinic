using BLL.Services;
using DAL.EF.Table;
using Microsoft.AspNetCore.Authorization;
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

        // Admin, Doctor, Patient all can view doctor list
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetDoctorsListAsync();
            return View(doctors);
        }

        // Only Admin can add doctor
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            ModelState.Remove("DoctorId");
            ModelState.Remove("Appointments");
            ModelState.Remove("Notifications");

            if (ModelState.IsValid)
            {
                await _doctorService.CreateDoctorAsync(doctor);
                TempData["SuccessMessage"] = "Doctor added successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(doctor);
        }

        // Only Admin can edit doctor
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }

            ModelState.Remove("Appointments");
            ModelState.Remove("Notifications");

            if (ModelState.IsValid)
            {
                await _doctorService.UpdateDoctorAsync(doctor);
                TempData["SuccessMessage"] = "Doctor updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(doctor);
        }

        // Only Admin can delete doctor
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _doctorService.DeleteDoctorAsync(id);
            TempData["SuccessMessage"] = "Doctor deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}