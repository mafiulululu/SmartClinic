using DAL.EF.Table;

namespace DAL.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<IEnumerable<Notification>> GetNotificationsByPatientEmailAsync(string email);
        Task<IEnumerable<Notification>> GetNotificationsByDoctorEmailAsync(string email);
        Task AddNotificationAsync(Notification notification);
    }
}