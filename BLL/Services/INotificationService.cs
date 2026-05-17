using DAL.EF.Table;

namespace BLL.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<IEnumerable<Notification>> GetPatientNotificationsAsync(string patientEmail);
        Task<IEnumerable<Notification>> GetDoctorNotificationsAsync(string doctorEmail);

        Task CreateAppointmentBookingNotificationAsync(
            Patient patient,
            Doctor doctor,
            DateTime appointmentDate
        );
    }
}