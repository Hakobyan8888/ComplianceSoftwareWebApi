using ComplianceSoftwareWebApi.Models;
using System.Security.Claims;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        string GetUserIdFromClaims(ClaimsPrincipal user);
        Task<bool> HasPermissionAsync(string userId, PermissionTypes permissionTypes);
    }

}
