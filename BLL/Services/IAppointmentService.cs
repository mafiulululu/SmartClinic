using DAL.EF.Table;

namespace BLL.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(string patientEmail);
        Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(string doctorEmail);

        Task<Appointment?> GetAppointmentByIdAsync(int id);

        Task<(bool IsSuccess, string Message)> BookAppointmentAsync(
            string patientEmail,
            int doctorId,
            DateTime appointmentDate,
            string symptomsNotes
        );

        Task<(bool IsSuccess, string Message)> UpdateAppointmentStatusAsync(
            int appointmentId,
            string status
        );
    }
}