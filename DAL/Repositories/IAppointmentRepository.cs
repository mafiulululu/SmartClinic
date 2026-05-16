using DAL.EF.Table;

namespace DAL.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientEmailAsync(string email);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorEmailAsync(string email);

        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<Patient?> GetPatientByEmailAsync(string email);
        Task<Doctor?> GetDoctorByIdAsync(int doctorId);

        Task<bool> IsDoctorDoubleBookedAsync(int doctorId, DateTime appointmentDate);

        Task AddAppointmentAsync(Appointment appointment);
    }
}