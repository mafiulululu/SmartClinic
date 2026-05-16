using DAL.EF.Table;
using DAL.Repositories;

namespace BLL.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsListAsync()
        {
            return await _doctorRepository.GetAllDoctorsAsync();
        }

        public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(
            string? speciality,
            decimal? minFee,
            decimal? maxFee,
            bool availableOnly
        )
        {
            speciality = speciality?.Trim();

            if (minFee.HasValue && minFee.Value < 0)
            {
                minFee = 0;
            }

            if (maxFee.HasValue && maxFee.Value < 0)
            {
                maxFee = null;
            }

            if (minFee.HasValue && maxFee.HasValue && minFee.Value > maxFee.Value)
            {
                var temp = minFee;
                minFee = maxFee;
                maxFee = temp;
            }

            return await _doctorRepository.SearchDoctorsAsync(
                speciality,
                minFee,
                maxFee,
                availableOnly
            );
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _doctorRepository.GetDoctorByIdAsync(id);
        }

        public async Task CreateDoctorAsync(Doctor doctor)
        {
            doctor.FirstName = doctor.FirstName.Trim();
            doctor.LastName = doctor.LastName.Trim();
            doctor.Speciality = doctor.Speciality.Trim();
            doctor.Email = doctor.Email.Trim().ToLower();

            await _doctorRepository.AddDoctorAsync(doctor);
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            doctor.FirstName = doctor.FirstName.Trim();
            doctor.LastName = doctor.LastName.Trim();
            doctor.Speciality = doctor.Speciality.Trim();
            doctor.Email = doctor.Email.Trim().ToLower();

            await _doctorRepository.UpdateDoctorAsync(doctor);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            await _doctorRepository.DeleteDoctorAsync(id);
        }
    }
}