using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartClinic.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetPatientsListAsync();
            return View(patients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientDTO patientDto)
        {
            ModelState.Remove("PatientId");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                await _patientService.CreatePatientAsync(patientDto);
                TempData["SuccessMessage"] = "Patient added successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(patientDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientDTO patientDto)
        {
            if (id != patientDto.PatientId)
            {
                return BadRequest();
            }

            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                await _patientService.UpdatePatientAsync(patientDto);
                TempData["SuccessMessage"] = "Patient information updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(patientDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _patientService.DeletePatientAsync(id);
            TempData["SuccessMessage"] = "Patient deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}