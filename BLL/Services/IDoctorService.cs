using DAL.EF.Table;


namespace BLL.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetDoctorsListAsync();
        Task CreateDoctorAsync(Doctor doctor);
    }
}