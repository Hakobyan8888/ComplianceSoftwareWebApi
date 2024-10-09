using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ComplianceSoftwareWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Grants a permission to a user. Accessible by Owner or Manager roles.
        /// </summary>
        /// <param name="dto">Data transfer object containing the permission details.</param>
        /// <returns>Returns 200 OK if the permission is granted successfully.</returns>
        /// <exception cref="CustomApplicationException">Thrown if an error occurs during the permission grant process.</exception>
        [HttpPost("grant")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> GrantPermission(GrantRemovePermissionDto dto)
        {
            try
            {
                await _permissionService.GrantPermissionAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Removes a permission from a user. Accessible by Owner or Manager roles.
        /// </summary>
        /// <param name="dto">Data transfer object containing the permission details.</param>
        /// <returns>Returns 200 OK if the permission is removed successfully.</returns>
        /// <exception cref="CustomApplicationException">Thrown if an error occurs during the permission removal process.</exception>
        [HttpPost("remove")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> RemovePermission(GrantRemovePermissionDto dto)
        {
            try
            {
                await _permissionService.RemovePermissionAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all available permission types.
        /// </summary>
        /// <returns>Returns a list of all permission types.</returns>
        /// <exception cref="CustomApplicationException">Thrown if an error occurs while retrieving the permission types.</exception>
        [HttpGet("GetAllPermissionTypes")]
        [Authorize]
        public async Task<IActionResult> GetAllPermissionTypes()
        {
            try
            {
                var permissions = await _permissionService.GetAllPermissionTypes();
                return Ok(JsonSerializer.Serialize(permissions));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

}
