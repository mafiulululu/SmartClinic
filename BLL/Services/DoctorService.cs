using BLL.DTOs;
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

        public async Task<IEnumerable<DoctorDTO>> GetDoctorsListAsync()
        {
            var doctors = await _doctorRepository.GetAllDoctorsAsync();

            return doctors.Select(MapperConfig.ToDoctorDTO).ToList();
        }

        public async Task<IEnumerable<DoctorDTO>> SearchDoctorsAsync(
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

            var doctors = await _doctorRepository.SearchDoctorsAsync(
                speciality,
                minFee,
                maxFee,
                availableOnly
            );

            return doctors.Select(MapperConfig.ToDoctorDTO).ToList();
        }

        public async Task<DoctorDTO?> GetDoctorByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return null;
            }

            return MapperConfig.ToDoctorDTO(doctor);
        }

        public async Task CreateDoctorAsync(DoctorDTO doctorDto)
        {
            doctorDto.FirstName = doctorDto.FirstName.Trim();
            doctorDto.LastName = doctorDto.LastName.Trim();
            doctorDto.Speciality = doctorDto.Speciality.Trim();
            doctorDto.Email = doctorDto.Email.Trim().ToLower();

            var doctor = MapperConfig.ToDoctorEntity(doctorDto);

            await _doctorRepository.AddDoctorAsync(doctor);
        }

        public async Task UpdateDoctorAsync(DoctorDTO doctorDto)
        {
            doctorDto.FirstName = doctorDto.FirstName.Trim();
            doctorDto.LastName = doctorDto.LastName.Trim();
            doctorDto.Speciality = doctorDto.Speciality.Trim();
            doctorDto.Email = doctorDto.Email.Trim().ToLower();

            var doctor = MapperConfig.ToDoctorEntity(doctorDto);

            await _doctorRepository.UpdateDoctorAsync(doctor);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            await _doctorRepository.DeleteDoctorAsync(id);
        }
    }
}