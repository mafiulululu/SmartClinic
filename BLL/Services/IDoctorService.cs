using DAL.EF.Table;

namespace BLL.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetDoctorsListAsync();

        Task<IEnumerable<Doctor>> SearchDoctorsAsync(
            string? speciality,
            decimal? minFee,
            decimal? maxFee,
            bool availableOnly
        );

        Task<Doctor?> GetDoctorByIdAsync(int id);
        Task CreateDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(int id);
    }
}