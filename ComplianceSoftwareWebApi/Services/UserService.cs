using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services.Interfaces;
using ComplianceSoftwareWebApi.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ComplianceSoftwareWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetByEmailAsync(email);
        }
        public async Task<User> GetUserById(string id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public string GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim;
        }

        public async Task<bool> HasPermissionAsync(string userId, PermissionTypes permissionTypes)
        {
            var permission = await _unitOfWork.Permissions.GetPermissionByPermissionType(permissionTypes);
            if (permission == null)
            {
                return false;
            }
            // Assuming you have a way to check permissions in your database
            var permissionAvailable = await _unitOfWork.UserPermissions.UserHasPermissionAsync(userId, permission);

            return permissionAvailable;
        }
    }

}
