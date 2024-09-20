using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceSoftwareWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPermissionService _permissionService;

        public AuthController(IAuthService authService, IPermissionService permissionService)
        {
            _authService = authService;
            _permissionService = permissionService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = await _authService.RegisterAsync(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(new { Token = token });
        }

        // Add a user (accessible by Owners and users with the right permission)
        [HttpPost("add-user")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> AddUser(RegisterDto dto)
        {
            // Check if the current user is the owner
            if (User.IsInRole("Owner"))
            {
                // Owners can always add users
                await _authService.AddUserToCompanyAsync(dto);
                return Ok();
            }

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(User, PermissionTypes.AddUser))
            {
                return Forbid("You do not have permission to add users.");
            }

            // If the user has permission, allow them to add a new user
            await _authService.RegisterAsync(dto);
            return Ok();
        }
    }

}
