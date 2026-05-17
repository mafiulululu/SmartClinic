using DAL.EF;
using DAL.EF.Table;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly SmartClinicContext _context;

        public PatientRepository(SmartClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .OrderBy(p => p.FirstName)
                .ToListAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task<Patient?> GetPatientByEmailAsync(string email)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Patients
                .AnyAsync(p => p.Email == email);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            var existingPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);

            if (existingPatient == null)
            {
                return;
            }

            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.Email = patient.Email;
            existingPatient.Phone = patient.Phone;
            existingPatient.Dob = patient.Dob;

            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return;
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}