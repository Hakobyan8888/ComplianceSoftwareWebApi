using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Permission> GetPermissionByPermissionType(PermissionTypes permissionTypes)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(x => x.PermissionType == permissionTypes);
            return permission;
        }

        public async Task<bool> UserHasPermissionAsync(string userId, PermissionTypes permissionTypes, int companyId)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(x => x.PermissionType == permissionTypes);
            if (permission == null)
            {
                return false;
            }
            return await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission == permission && up.User.CompanyId == companyId);
        }
    }

}
