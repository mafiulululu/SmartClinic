using DAL.EF.Table;
using DAL.Repositories;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        private readonly string[] _allowedRoles =
        {
            "Admin",
            "Doctor",
            "Patient"
        };

        public AuthService(
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository
        )
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<(bool IsSuccess, string Message)> RegisterUserAsync(
            User user,
            string? phone,
            DateOnly? dob,
            string? speciality,
            decimal? consultationFee,
            bool isAvailable
        )
        {
            user.FullName = user.FullName.Trim();
            user.Email = user.Email.Trim().ToLower();
            user.Role = user.Role.Trim();

            if (!_allowedRoles.Contains(user.Role))
            {
                return (false, "Invalid role selected.");
            }

            if (await _userRepository.EmailExistsAsync(user.Email))
            {
                return (false, "This email is already registered.");
            }

            if (user.Role == "Patient")
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    return (false, "Phone number is required for patient registration.");
                }

                if (!dob.HasValue)
                {
                    return (false, "Date of birth is required for patient registration.");
                }

                if (await _patientRepository.EmailExistsAsync(user.Email))
                {
                    return (false, "A patient profile already exists with this email.");
                }
            }

            if (user.Role == "Doctor")
            {
                if (string.IsNullOrWhiteSpace(speciality))
                {
                    return (false, "Speciality is required for doctor registration.");
                }

                if (!consultationFee.HasValue || consultationFee.Value <= 0)
                {
                    return (false, "Consultation fee is required for doctor registration.");
                }

                if (await _doctorRepository.EmailExistsAsync(user.Email))
                {
                    return (false, "A doctor profile already exists with this email.");
                }
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.Now;

            await _userRepository.AddUserAsync(user);

            var nameParts = user.FullName.Split(
                ' ',
                2,
                StringSplitOptions.RemoveEmptyEntries
            );

            string firstName = nameParts.Length > 0 ? nameParts[0] : user.FullName;
            string lastName = nameParts.Length > 1 ? nameParts[1] : "N/A";

            if (user.Role == "Patient")
            {
                var patient = new Patient
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = user.Email,
                    Phone = phone!.Trim(),
                    Dob = dob!.Value,
                    CreatedAt = DateTime.Now
                };

                await _patientRepository.AddPatientAsync(patient);
            }

            if (user.Role == "Doctor")
            {
                var doctor = new Doctor
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = user.Email,
                    Speciality = speciality!.Trim(),
                    ConsultationFee = consultationFee!.Value,
                    IsAvailable = isAvailable
                };

                await _doctorRepository.AddDoctorAsync(doctor);
            }

            return (true, "Registration successful. Please login.");
        }

        public async Task<User?> LoginUserAsync(string email, string password, string role)
        {
            email = email.Trim().ToLower();
            role = role.Trim();

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            if (user.Role != role)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            return user;
        }
    }
}