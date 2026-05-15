using DAL.EF.Table;
using DAL.Repositories;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly string[] _allowedRoles =
        {
            "Admin",
            "Doctor",
            "Patient"
        };

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            user.Email = user.Email.Trim().ToLower();
            user.Role = user.Role.Trim();

            if (!_allowedRoles.Contains(user.Role))
            {
                return false;
            }

            // Business Rule: Prevent duplicate email
            if (await _userRepository.EmailExistsAsync(user.Email))
            {
                return false;
            }

            // Hash password before saving
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.Now;

            await _userRepository.AddUserAsync(user);
            return true;
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