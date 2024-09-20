using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class UserPermissionRepository : Repository<UserPermission>, IUserPermissionRepository
    {
        public UserPermissionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> UserHasPermissionAsync(string userId, Permission permission, int companyId)
        {
            if (permission == null)
            {
                return false;
            }
            return await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission == permission && up.User.CompanyId == companyId);
        }

        public async Task<UserPermission> GetUserPermission(string userId, int permissionId)
        {
            return await _context.UserPermissions.FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);
        }
    }
}
