using DAL.EF.Table;
using DAL.Repositories;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            // Business Rule: Prevent duplicate emails
            if (await _userRepository.EmailExistsAsync(user.Email))
            {
                return false;
            }

            await _userRepository.AddUserAsync(user);
            return true;
        }
    }
}