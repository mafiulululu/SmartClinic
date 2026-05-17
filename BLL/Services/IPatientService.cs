using BLL.DTOs;

namespace BLL.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDTO>> GetPatientsListAsync();

        Task<PatientDTO?> GetPatientByIdAsync(int id);

        Task CreatePatientAsync(PatientDTO patientDto);

        Task UpdatePatientAsync(PatientDTO patientDto);

        Task DeletePatientAsync(int id);
    }
}