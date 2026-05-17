using BLL.DTOs;
using DAL.Repositories;

namespace BLL.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientDTO>> GetPatientsListAsync()
        {
            var patients = await _patientRepository.GetAllPatientsAsync();

            return patients.Select(MapperConfig.ToPatientDTO).ToList();
        }

        public async Task<PatientDTO?> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);

            if (patient == null)
            {
                return null;
            }

            return MapperConfig.ToPatientDTO(patient);
        }

        public async Task CreatePatientAsync(PatientDTO patientDto)
        {
            patientDto.FirstName = patientDto.FirstName.Trim();
            patientDto.LastName = patientDto.LastName.Trim();
            patientDto.Email = patientDto.Email.Trim().ToLower();
            patientDto.Phone = patientDto.Phone.Trim();
            patientDto.CreatedAt = DateTime.Now;

            var patient = MapperConfig.ToPatientEntity(patientDto);

            await _patientRepository.AddPatientAsync(patient);
        }

        public async Task UpdatePatientAsync(PatientDTO patientDto)
        {
            patientDto.FirstName = patientDto.FirstName.Trim();
            patientDto.LastName = patientDto.LastName.Trim();
            patientDto.Email = patientDto.Email.Trim().ToLower();
            patientDto.Phone = patientDto.Phone.Trim();

            var patient = MapperConfig.ToPatientEntity(patientDto);

            await _patientRepository.UpdatePatientAsync(patient);
        }

        public async Task DeletePatientAsync(int id)
        {
            await _patientRepository.DeletePatientAsync(id);
        }
    }
}