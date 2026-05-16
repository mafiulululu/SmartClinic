using DAL.EF.Table;

namespace DAL.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<IEnumerable<Doctor>> SearchDoctorsAsync(
            string? speciality,
            decimal? minFee,
            decimal? maxFee,
            bool availableOnly
        );

        Task<Doctor?> GetDoctorByIdAsync(int id);
        Task AddDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task DeleteDoctorAsync(int id);
    }
}