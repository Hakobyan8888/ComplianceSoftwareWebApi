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
        /// <summary>
        /// Grants a permission to a user.
        /// </summary>
        /// <param name="dto">Data transfer object containing the permission details.</param>
        /// <exception cref="CustomApplicationException">Thrown if user or permission is not found or if any error occurs during the operation.</exception>
        public async Task GrantPermissionAsync(GrantRemovePermissionDto dto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
                var permission = await _unitOfWork.Permissions.GetPermissionByPermissionType((PermissionTypes)dto.PermissionId);

                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {dto.UserId} not found.");
                }

                if (permission == null)
                {
                    throw new KeyNotFoundException($"Permission with ID {dto.PermissionId} not found.");
                }

                var userPermission = new UserPermission { UserId = user.Id, PermissionId = permission.Id };
                await _unitOfWork.UserPermissions.AddAsync(userPermission);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Removes a permission from a user.
        /// </summary>
        /// <param name="dto">Data transfer object containing the permission details.</param>
        /// <exception cref="CustomApplicationException">Thrown if user or permission is not found or if any error occurs during the operation.</exception>
        public async Task RemovePermissionAsync(GrantRemovePermissionDto dto)
        {
            try
            {
                var permission = await _unitOfWork.Permissions.GetPermissionByPermissionType((PermissionTypes)dto.PermissionId);
                if (permission == null)
                {
                    throw new KeyNotFoundException($"Permission with ID {dto.PermissionId} not found.");
                }

                var userPermission = await _unitOfWork.UserPermissions.GetUserPermission(dto.UserId, permission.Id);
                if (userPermission == null)
                {
                    throw new KeyNotFoundException($"User with ID {dto.UserId} does not have permission ID {dto.PermissionId}.");
                }

                await _unitOfWork.UserPermissions.DeleteAsync(userPermission.Id);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if a user has a specific permission.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="permissionTypes">The type of permission.</param>
        /// <returns>Returns true if the user has the specified permission.</returns>
        /// <exception cref="CustomApplicationException">Thrown if any error occurs during the operation.</exception>
        public async Task<bool> HasPermissionAsync(string userId, PermissionTypes permissionTypes)
        {
            try
            {
                return await _userService.HasPermissionAsync(userId, permissionTypes);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all permission types.
        /// </summary>
        /// <returns>Returns a list of all available permissions.</returns>
        /// <exception cref="CustomApplicationException">Thrown if any error occurs during the operation.</exception>
        public async Task<IEnumerable<Permission>> GetAllPermissionTypes()
        {
            try
            {
                var permissions = await _unitOfWork.Permissions.GetAllAsync();
                return permissions;
            }
            catch
            {
                throw;
            }
        }

    }

}
