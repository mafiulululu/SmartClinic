using DAL.EF;
using DAL.EF.Table;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly SmartClinicContext _context;

        public DoctorRepository(SmartClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                .OrderBy(d => d.FirstName)
                .ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == id);
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            var existingDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctor.DoctorId);

            if (existingDoctor == null)
            {
                return;
            }

            existingDoctor.FirstName = doctor.FirstName;
            existingDoctor.LastName = doctor.LastName;
            existingDoctor.Speciality = doctor.Speciality;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.Email = doctor.Email;
            existingDoctor.IsAvailable = doctor.IsAvailable;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
            {
                return;
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}