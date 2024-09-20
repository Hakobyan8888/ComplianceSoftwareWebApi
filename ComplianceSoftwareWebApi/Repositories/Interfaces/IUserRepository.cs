using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<int> GetCompanyIdForUserAsync(string userId);
    }

}
