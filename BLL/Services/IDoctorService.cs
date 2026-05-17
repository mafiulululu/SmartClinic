using BLL.DTOs;

namespace BLL.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDTO>> GetDoctorsListAsync();

        Task<IEnumerable<DoctorDTO>> SearchDoctorsAsync(
            string? speciality,
            decimal? minFee,
            decimal? maxFee,
            bool availableOnly
        );

        Task<DoctorDTO?> GetDoctorByIdAsync(int id);

        Task CreateDoctorAsync(DoctorDTO doctorDto);

        Task UpdateDoctorAsync(DoctorDTO doctorDto);

        Task DeleteDoctorAsync(int id);
    }
}