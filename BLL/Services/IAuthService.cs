using DAL.EF.Table;

namespace BLL.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(User user);
    }
}