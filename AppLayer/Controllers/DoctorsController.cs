using BLL.DTOs;
using BLL.Services;
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

        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> Index(
            string? speciality,
            decimal? minFee,
            decimal? maxFee,
            bool availableOnly = false
        )
        {
            var doctors = await _doctorService.SearchDoctorsAsync(
                speciality,
                minFee,
                maxFee,
                availableOnly
            );

            ViewBag.Speciality = speciality;
            ViewBag.MinFee = minFee;
            ViewBag.MaxFee = maxFee;
            ViewBag.AvailableOnly = availableOnly;

            return View(doctors);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorDTO doctorDto)
        {
            ModelState.Remove("DoctorId");

            if (ModelState.IsValid)
            {
                await _doctorService.CreateDoctorAsync(doctorDto);
                TempData["SuccessMessage"] = "Doctor added successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(doctorDto);
        }

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
        public async Task<IActionResult> Edit(int id, DoctorDTO doctorDto)
        {
            if (id != doctorDto.DoctorId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _doctorService.UpdateDoctorAsync(doctorDto);
                TempData["SuccessMessage"] = "Doctor information updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(doctorDto);
        }

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