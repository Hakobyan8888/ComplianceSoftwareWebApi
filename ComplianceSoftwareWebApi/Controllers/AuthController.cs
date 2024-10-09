using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// The user is automatically assigned the 'Owner' role upon successful registration.
        /// </summary>
        /// <param name="dto">The registration details including email, password, and other user information.</param>
        /// <returns>
        /// An HTTP response indicating the result of the registration process.
        /// - 200 OK: If the registration is successful.
        /// - 400 Bad Request: If there are validation errors, the user creation fails, or role assignment fails.
        /// - 500 Internal Server Error: If the registration fails due to an internal server issue.
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                // Input validation, check if model is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 with validation errors
                }

                // Assign the default role to the user
                dto.Role = Roles.Owner;

                // Register the user with AuthService
                var user = await _authService.RegisterAsync(dto);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "User registration failed.");
                }

                // Create the user using UserManager
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors); // Return 400 if UserManager creation fails
                }

                // Assign role to the user
                var roleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors); // Return 400 if role assignment fails
                }

                // Return success
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary (this will also be logged by your middleware)
                // _logger.LogError(ex, "Error occurred during user registration.");

                // Allow the middleware to handle the exception
                throw;
            }
        }

        /// <summary>
        /// Logs in a user by validating their credentials and generating a JWT token.
        /// </summary>
        /// <param name="dto">The login details including email and password.</param>
        /// <returns>
        /// An HTTP response containing the JWT token if login is successful,
        /// or an Unauthorized response if the credentials are invalid.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                // Check if the user exists by email and retrieve their roles
                var user = await _userService.GetUserByEmailAsync(dto.Email);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials."); // Return 401 if the user does not exist
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = await _authService.LoginAsync(dto, roles.ToList());

                // Return the generated token
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception to be handled by the middleware
                throw;
            }
        }


        /// <summary>
        /// Adds a new user to the system, accessible by Owners and Managers with the correct permissions.
        /// </summary>
        /// <param name="dto">The registration details for the new user.</param>
        /// <returns>
        /// An HTTP response indicating the result of the operation.
        /// - 200 OK: If the user is successfully added.
        /// - 403 Forbidden: If the user does not have permission to add users.
        /// - 400 Bad Request: If user registration fails.
        /// - 500 Internal Server Error: If the registration fails due to an internal issue.
        [HttpPost("add-user")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> AddUser(RegisterDto dto)
        {
            try
            {
                // Retrieve current user's ID from claims
                var userId = _userService.GetUserIdFromClaims(User);

                // Check if the user has permission to add new users
                if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
                {
                    return Forbid("You do not have permission to add users.");
                }

                // Check if the user has registered a company
                var baseUser = await _userService.GetUserById(userId);
                if (baseUser.CompanyId == null)
                {
                    return Forbid("Please register a company before adding users.");
                }

                // Assign the user's company ID to the new user being added
                dto.CompanyId = Convert.ToInt32(baseUser.CompanyId);

                // Register the new user
                var user = await _authService.RegisterAsync(dto);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to register the user.");
                }

                // Create the user using UserManager and assign a role
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Assign the appropriate role
                var roleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                // Return success
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception to be handled by the middleware
                throw;
            }
        }

        /// <summary>
        /// Retrieves a list of users associated with the current user's company.
        /// </summary>
        /// <returns>
        /// An HTTP response containing the list of users if successful,
        /// or a NotFound response if the company is not found.
        /// </returns>
        [HttpGet("get-users")]
        [Authorize]
        public async Task<IActionResult> GetUsersByCompanyId()
        {
            try
            {
                // Retrieve current user's ID and company ID
                var userId = _userService.GetUserIdFromClaims(User);
                var companyId = (await _userService.GetUserById(userId)).CompanyId;

                if (companyId == null)
                {
                    return NotFound("Company not found.");
                }

                // Fetch the users associated with the company
                var users = await _authService.GetUsersByCompanyId((int)companyId);

                // Return the users
                return Ok(JsonSerializer.Serialize(users));
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception to be handled by the middleware
                throw;
            }
        }

        [HttpDelete("delete-user-employee")]
        [Authorize(Roles = "Owner,Manager")]
        public async Task<IActionResult> DeleteUserEmployee(string email)
        {
            var isSucceeded = await _authService.DeleteUser(email);
            return Ok(isSucceeded);
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
    }

}
