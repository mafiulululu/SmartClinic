using DAL.EF;
using DAL.EF.Table;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly SmartClinicContext _context;

        public InvoiceRepository(SmartClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Doctor)
                .OrderByDescending(i => i.GeneratedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByPatientEmailAsync(string email)
        {
            email = email.Trim().ToLower();

            return await _context.Invoices
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Where(i => i.Appointment.Patient.Email.ToLower() == email)
                .OrderByDescending(i => i.GeneratedAt)
                .ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Doctor)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentStatusAsync(int invoiceId)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

            if (invoice == null)
            {
                return;
            }

            invoice.PaymentStatus = "Paid";

            await _context.SaveChangesAsync();
        }
    }
}