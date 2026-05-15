using DAL.EF.Table;

namespace BLL.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User?> LoginUserAsync(string email, string password, string role);
    }
}