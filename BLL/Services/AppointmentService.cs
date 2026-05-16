using DAL.EF.Table;
using DAL.Repositories;

namespace BLL.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(string patientEmail)
        {
            patientEmail = patientEmail.Trim().ToLower();
            return await _appointmentRepository.GetAppointmentsByPatientEmailAsync(patientEmail);
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(string doctorEmail)
        {
            doctorEmail = doctorEmail.Trim().ToLower();
            return await _appointmentRepository.GetAppointmentsByDoctorEmailAsync(doctorEmail);
        }

        public async Task<(bool IsSuccess, string Message)> BookAppointmentAsync(
            string patientEmail,
            int doctorId,
            DateTime appointmentDate,
            string symptomsNotes
        )
        {
            patientEmail = patientEmail.Trim().ToLower();
            symptomsNotes = symptomsNotes.Trim();

            var patient = await _appointmentRepository.GetPatientByEmailAsync(patientEmail);

            if (patient == null)
            {
                return (false, "No patient profile found for this logged-in email. Please contact admin.");
            }

            var doctor = await _appointmentRepository.GetDoctorByIdAsync(doctorId);

            if (doctor == null)
            {
                return (false, "Selected doctor was not found.");
            }

            if (!doctor.IsAvailable)
            {
                return (false, "This doctor is currently unavailable for appointments.");
            }

            if (appointmentDate <= DateTime.Now)
            {
                return (false, "Appointment date and time must be in the future.");
            }

            bool isDoubleBooked = await _appointmentRepository
                .IsDoctorDoubleBookedAsync(doctorId, appointmentDate);

            if (isDoubleBooked)
            {
                return (false, "This doctor already has an appointment at the selected time.");
            }

            var appointment = new Appointment
            {
                PatientId = patient.PatientId,
                DoctorId = doctor.DoctorId,
                AppointmentDate = appointmentDate,
                SymptomsNotes = symptomsNotes,
                Status = "Pending"
            };

            await _appointmentRepository.AddAppointmentAsync(appointment);

            return (true, "Appointment booked successfully.");
        }
    }
}