using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceSoftwareWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(IAuthService authService,
            IPermissionService permissionService,
            IUserService userService,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _permissionService = permissionService;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            dto.Role = Roles.Owner;
            var user = await _authService.RegisterAsync(dto);
            var result = await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var a = await _userManager.GetRolesAsync(await _userService.GetUserByEmailAsync(dto.Email));
            var token = await _authService.LoginAsync(dto, a.ToList());
            return Ok(new { Token = token });
        }

        [HttpPost("login/cookies")]
        public async Task<IActionResult> LoginCookies(LoginDto dto)
        {
            var a = await _userManager.GetRolesAsync(await _userService.GetUserByEmailAsync(dto.Email));
            var token = await _authService.LoginAsync(dto, a.ToList());
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access
                Secure = true, // Ensure this is set to true in production with HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            // Set the JWT token in an HTTP-only cookie
            Response.Cookies.Append("AuthToken", token, cookieOptions);
            return Ok();
        }

        // Add a user (accessible by Owners and users with the right permission)
        [HttpPost("add-user")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> AddUser(RegisterDto dto)
        {
            // Check if the current user is the owner
            var userId = _userService.GetUserIdFromClaims(User);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }
            var baseUser = await _userService.GetUserById(userId);
            if (baseUser.CompanyId == null)
            {
                return Forbid("Please register Company before adding users");
            }
            dto.CompanyId = Convert.ToInt32(baseUser.CompanyId);

            // If the user has permission, allow them to add a new user
            var user = await _authService.RegisterAsync(dto);
            var result = await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            return Ok(user);
        }
    }

}
