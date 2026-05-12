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

        
        public async Task CreateDoctorAsync(Doctor doctor)
        {
            
            await _doctorRepository.AddDoctorAsync(doctor);
        }
    }
}