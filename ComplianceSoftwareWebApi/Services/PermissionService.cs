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
            var permission = await _unitOfWork.Permissions.GetPermissionByPermissionType((PermissionTypes)dto.PermissionId);

            if (user != null && permission != null)
            {
                var userPermission = new UserPermission { UserId = user.Id, PermissionId = permission.Id };
                await _unitOfWork.UserPermissions.AddAsync(userPermission);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task RemovePermissionAsync(GrantRemovePermissionDto dto)
        {
            var permission = await _unitOfWork.Permissions.GetPermissionByPermissionType((PermissionTypes)dto.PermissionId);
            var userPermission = await _unitOfWork.UserPermissions.GetUserPermission(dto.UserId, permission.Id);
            if (userPermission != null)
            {
                await _unitOfWork.UserPermissions.DeleteAsync(userPermission.Id);
                await _unitOfWork.CompleteAsync();
            }
        }
        public async Task<bool> HasPermissionAsync(string userId, PermissionTypes permissionTypes)
        {
            return await _userService.HasPermissionAsync(userId, permissionTypes);
        }
    }

}
