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

        [HttpPost("grant")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> GrantPermission(GrantRemovePermissionDto dto)
        {
            await _permissionService.GrantPermissionAsync(dto);
            return Ok();
        }

        [HttpPost("remove")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> RemovePermission(GrantRemovePermissionDto dto)
        {
            await _permissionService.RemovePermissionAsync(dto);
            return Ok();
        }

        [HttpPost("GetAllPermissionTypes")]
        [Authorize]
        public async Task<IActionResult> GetAllPermissionTypes()
        {
            var permissions = await _permissionService.GetAllPermissionTypes();
            return Ok(JsonSerializer.Serialize(permissions));
        }
    }

}
