using DAL.EF.Table;
using DAL.Repositories;

namespace BLL.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _invoiceRepository.GetAllInvoicesAsync();
        }

        public async Task<IEnumerable<Invoice>> GetPatientInvoicesAsync(string patientEmail)
        {
            patientEmail = patientEmail.Trim().ToLower();
            return await _invoiceRepository.GetInvoicesByPatientEmailAsync(patientEmail);
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _invoiceRepository.GetInvoiceByIdAsync(id);
        }

        public async Task CreateInvoiceForAppointmentAsync(
            int appointmentId,
            decimal consultationFee
        )
        {
            decimal baseAmount = consultationFee;
            decimal taxAmount = Math.Round(baseAmount * 0.05m, 2);
            decimal discount = 0;
            decimal totalAmount = baseAmount + taxAmount - discount;

            var invoice = new Invoice
            {
                AppointmentId = appointmentId,
                BaseAmount = baseAmount,
                TaxAmount = taxAmount,
                Discount = discount,
                TotalAmount = totalAmount,
                PaymentStatus = "Unpaid",
                GeneratedAt = DateTime.Now
            };

            await _invoiceRepository.AddInvoiceAsync(invoice);
        }

        public async Task<(bool IsSuccess, string Message)> PayInvoiceAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
            {
                return (false, "Invoice not found.");
            }

            if (invoice.PaymentStatus == "Paid")
            {
                return (false, "This invoice is already paid.");
            }

            await _invoiceRepository.UpdatePaymentStatusAsync(invoiceId);

            return (true, "Payment completed successfully.");
        }
    }
}