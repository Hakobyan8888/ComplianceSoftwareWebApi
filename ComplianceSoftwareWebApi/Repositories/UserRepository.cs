using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<int> GetCompanyIdForUserAsync(string userId)
        {
            return (int)await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.CompanyId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsersByCompanyId(int companyId)
        {
            return await _context.Users.Where(x => x.CompanyId == companyId).ToListAsync();
        }
    }

}
