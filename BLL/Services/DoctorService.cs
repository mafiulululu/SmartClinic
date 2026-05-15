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