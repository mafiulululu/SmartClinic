using DAL.EF.Table;

namespace BLL.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetPatientsListAsync();
        Task<Patient?> GetPatientByIdAsync(int id);
        Task CreatePatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(int id);
    }
}