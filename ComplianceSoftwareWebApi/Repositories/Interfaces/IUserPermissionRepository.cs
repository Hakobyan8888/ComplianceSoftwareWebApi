using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Repositories.Interfaces
{
    public interface IUserPermissionRepository : IRepository<UserPermission>
    {
        Task<bool> UserHasPermissionAsync(string userId, Permission permissionTypes);
        Task<UserPermission> GetUserPermission(string userId, int permissionId);
    }
}
