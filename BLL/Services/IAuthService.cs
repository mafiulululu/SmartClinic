using DAL.EF.Table;

namespace BLL.Services
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, string Message)> RegisterUserAsync(
            User user,
            string? phone,
            DateOnly? dob
        );

        Task<User?> LoginUserAsync(string email, string password, string role);
    }
}