using DAL.EF.Table;
using DAL.Repositories;

namespace BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _notificationRepository.GetAllNotificationsAsync();
        }

        public async Task<IEnumerable<Notification>> GetPatientNotificationsAsync(string patientEmail)
        {
            patientEmail = patientEmail.Trim().ToLower();
            return await _notificationRepository.GetNotificationsByPatientEmailAsync(patientEmail);
        }

        public async Task<IEnumerable<Notification>> GetDoctorNotificationsAsync(string doctorEmail)
        {
            doctorEmail = doctorEmail.Trim().ToLower();
            return await _notificationRepository.GetNotificationsByDoctorEmailAsync(doctorEmail);
        }

        public async Task CreateAppointmentBookingNotificationAsync(
            Patient patient,
            Doctor doctor,
            DateTime appointmentDate
        )
        {
            string message =
                $"Appointment booked with Dr. {doctor.FirstName} {doctor.LastName} on {appointmentDate:yyyy-MM-dd hh:mm tt}.";

            if (message.Length > 200)
            {
                message = message.Substring(0, 200);
            }

            var notification = new Notification
            {
                PatientId = patient.PatientId,
                DoctorId = doctor.DoctorId,
                Message = message,
                Type = "Appointment",
                SendStatus = "Unread",
                ScheduledTime = DateTime.Now
            };

            await _notificationRepository.AddNotificationAsync(notification);
        }
    }
}