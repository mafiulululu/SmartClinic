using DAL.EF.Table;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}