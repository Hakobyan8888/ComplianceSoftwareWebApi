using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Repositories.Interfaces
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<bool> UserHasPermissionAsync(string userId, PermissionTypes permissionTypes, int companyId);
    }

}
