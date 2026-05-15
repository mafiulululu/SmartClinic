using DAL.EF.Table;

namespace BLL.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetDoctorsListAsync();
        Task<Doctor?> GetDoctorByIdAsync(int id);
        Task CreateDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(int id);
    }
}