using DAL.EF.Table;

namespace BLL.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<IEnumerable<Invoice>> GetPatientInvoicesAsync(string patientEmail);
        Task<Invoice?> GetInvoiceByIdAsync(int id);

        Task CreateInvoiceForAppointmentAsync(
            int appointmentId,
            decimal consultationFee
        );

        Task<(bool IsSuccess, string Message)> PayInvoiceAsync(int invoiceId);
    }
}