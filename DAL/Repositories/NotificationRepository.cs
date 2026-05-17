using DAL.EF;
using DAL.EF.Table;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SmartClinicContext _context;

        public NotificationRepository(SmartClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.Patient)
                .Include(n => n.Doctor)
                .OrderByDescending(n => n.ScheduledTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByPatientEmailAsync(string email)
        {
            return await _context.Notifications
                .Include(n => n.Patient)
                .Include(n => n.Doctor)
                .Where(n => n.Patient.Email == email)
                .OrderByDescending(n => n.ScheduledTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByDoctorEmailAsync(string email)
        {
            return await _context.Notifications
                .Include(n => n.Patient)
                .Include(n => n.Doctor)
                .Where(n => n.Doctor.Email == email)
                .OrderByDescending(n => n.ScheduledTime)
                .ToListAsync();
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
    }
}