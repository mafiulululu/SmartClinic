using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SmartClinic.Web.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return View(invoices);
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> MyInvoices()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var invoices = await _invoiceService.GetPatientInvoicesAsync(email);
            return View(invoices);
        }

        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Patient"))
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                if (invoice.Appointment.Patient.Email != email)
                {
                    return Forbid();
                }
            }

            return View(invoice);
        }
    }
}