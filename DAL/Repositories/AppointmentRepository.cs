using DAL.EF;
using DAL.EF.Table;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly SmartClinicContext _context;

        public AppointmentRepository(SmartClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientEmailAsync(string email)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Patient.Email == email)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorEmailAsync(string email)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Doctor.Email == email)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<Patient?> GetPatientByEmailAsync(string email)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int doctorId)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }

        public async Task<bool> IsDoctorDoubleBookedAsync(int doctorId, DateTime appointmentDate)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate == appointmentDate &&
                    a.Status != "Cancelled"
                );
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }
    }
}