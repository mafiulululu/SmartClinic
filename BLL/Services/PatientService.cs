using DAL.EF.Table;
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

        public async Task<IEnumerable<Patient>> GetPatientsListAsync()
        {
            return await _patientRepository.GetAllPatientsAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetPatientByIdAsync(id);
        }

        public async Task CreatePatientAsync(Patient patient)
        {
            patient.FirstName = patient.FirstName.Trim();
            patient.LastName = patient.LastName.Trim();
            patient.Email = patient.Email.Trim().ToLower();
            patient.Phone = patient.Phone.Trim();
            patient.CreatedAt = DateTime.Now;

            await _patientRepository.AddPatientAsync(patient);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            patient.FirstName = patient.FirstName.Trim();
            patient.LastName = patient.LastName.Trim();
            patient.Email = patient.Email.Trim().ToLower();
            patient.Phone = patient.Phone.Trim();

            await _patientRepository.UpdatePatientAsync(patient);
        }

        public async Task DeletePatientAsync(int id)
        {
            await _patientRepository.DeletePatientAsync(id);
        }
    }
}