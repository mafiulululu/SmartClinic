using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Web.ViewModels;
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

        [Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> Pay(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            var email = User.FindFirstValue(ClaimTypes.Email);

            if (invoice.Appointment.Patient.Email != email)
            {
                return Forbid();
            }

            if (invoice.PaymentStatus == "Paid")
            {
                TempData["SuccessMessage"] = "This invoice is already paid.";
                return RedirectToAction("Details", new { id = invoice.InvoiceId });
            }

            var model = new PaymentViewModel
            {
                InvoiceId = invoice.InvoiceId,
                TotalAmount = invoice.TotalAmount
            };

            return View(model);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(PaymentViewModel model)
        {
            if (model.PaymentMethod == "bKash" || model.PaymentMethod == "Nagad")
            {
                if (string.IsNullOrWhiteSpace(model.MobileWalletNumber))
                {
                    ModelState.AddModelError("MobileWalletNumber", "Mobile number is required for bKash/Nagad payment.");
                }
            }

            if (model.PaymentMethod == "Card")
            {
                if (string.IsNullOrWhiteSpace(model.TransactionReference))
                {
                    ModelState.AddModelError("TransactionReference", "Card transaction reference is required.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var invoice = await _invoiceService.GetInvoiceByIdAsync(model.InvoiceId);

            if (invoice == null)
            {
                return NotFound();
            }

            var email = User.FindFirstValue(ClaimTypes.Email);

            if (invoice.Appointment.Patient.Email != email)
            {
                return Forbid();
            }

            var result = await _invoiceService.PayInvoiceAsync(model.InvoiceId);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = $"Payment done successfully using {model.PaymentMethod}.";
            return RedirectToAction("Details", new { id = model.InvoiceId });
        }
    }
}