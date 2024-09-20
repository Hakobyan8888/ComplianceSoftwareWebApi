using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services.Interfaces;
using ComplianceSoftwareWebApi.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ComplianceSoftwareWebApi.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(ApplicationDbContext context, IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task GrantPermissionAsync(GrantRemovePermissionDto dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            var permission = await _unitOfWork.Permissions.GetByIdAsync(dto.PermissionId);

            if (user != null && permission != null)
            {
                var userPermission = new UserPermission { UserId = user.Id, PermissionId = permission.PermissionId };
                await _unitOfWork.UserPermissions.AddAsync(userPermission);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task RemovePermissionAsync(GrantRemovePermissionDto dto)
        {
            var userPermission = await _unitOfWork.UserPermissions.GetUserPermission(dto.UserId, dto.PermissionId);
            if (userPermission != null)
            {
                await _unitOfWork.UserPermissions.DeleteAsync(dto.PermissionId);
                await _unitOfWork.CompleteAsync();
            }
        }
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, PermissionTypes permissionTypes)
        {
            var userId = _userService.GetUserIdFromClaims(user);
            return await _userService.HasPermissionAsync(userId, permissionTypes);
        }
    }

}
