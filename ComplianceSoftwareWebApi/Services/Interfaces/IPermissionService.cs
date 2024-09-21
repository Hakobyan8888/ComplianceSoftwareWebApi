using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using System.Security.Claims;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface IPermissionService
    {
        Task GrantPermissionAsync(GrantRemovePermissionDto dto);
        Task RemovePermissionAsync(GrantRemovePermissionDto dto);
        Task<bool> HasPermissionAsync(string userId, PermissionTypes permissionTypes);
    }

}
