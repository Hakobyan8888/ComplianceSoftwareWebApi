using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services.Interfaces;
using ComplianceSoftwareWebApi.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IPasswordHasher<User> passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerDto">The data transfer object containing user details.</param>
        /// <returns>Task that returns the registered user.</returns>
        /// <exception cref="CustomApplicationException">Thrown if an error occurs during registration.</exception>
        public Task<User> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var user = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    Role = registerDto.Role,
                    PasswordHash = _passwordHasher.HashPassword(null, registerDto.Password),
                    CompanyId = registerDto.CompanyId
                };

                return Task.FromResult(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during registration.", ex);
            }
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token upon successful login.
        /// </summary>
        /// <param name="loginDto">The data transfer object containing login details.</param>
        /// <param name="roles">List of roles assigned to the user.</param>
        /// <returns>JWT token as a string.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when login credentials are invalid.</exception>
        /// <exception cref="CustomApplicationException">Thrown when a general error occurs during login.</exception>
        public async Task<string> LoginAsync(LoginDto loginDto, List<string> roles)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
                if (user == null || _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginDto.Password) != PasswordVerificationResult.Success)
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }

                return _jwtTokenGenerator.GenerateToken(user, roles);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Login failed: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred during login.", ex);
            }
        }

        /// <summary>
        /// Adds a new user to a company.
        /// </summary>
        /// <param name="dto">The data transfer object containing user details.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the user already exists.</exception>
        /// <exception cref="CustomApplicationException">Thrown when an error occurs while adding a user.</exception>
        public async Task AddUserToCompanyAsync(RegisterDto dto)
        {
            try
            {
                var existingUser = await GetUserByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("User already exists.");
                }

                var newUser = new User
                {
                    Email = dto.Email,
                    Role = dto.Role,
                    CompanyId = dto.CompanyId,
                    PasswordHash = _passwordHasher.HashPassword(null, dto.Password)
                };

                await _unitOfWork.Users.AddAsync(newUser);
                await _unitOfWork.CompleteAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Unable to add user: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding a user to the company.", ex);
            }
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The user object if found, null otherwise.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
        /// <exception cref="CustomApplicationException">Thrown when an error occurs while fetching the user by email.</exception>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _unitOfWork.Users.GetByEmailAsync(email) ?? throw new KeyNotFoundException("User not found.");
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("Unable to find user: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user by email.", ex);
            }
        }

        /// <summary>
        /// Retrieves all users belonging to a company by company ID.
        /// </summary>
        /// <param name="companyId">The ID of the company.</param>
        /// <returns>List of users associated with the company.</returns>
        /// <exception cref="CustomApplicationException">Thrown when an error occurs while retrieving users by company ID.</exception>
        public async Task<List<User>> GetUsersByCompanyId(int companyId)
        {
            try
            {
                return await _unitOfWork.Users.GetAllUsersByCompanyId(companyId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving users by company ID.", ex);
            }
        }

        /// <summary>
        /// Deletes a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to delete.</param>
        /// <returns>Boolean indicating whether the user was successfully deleted.</returns>
        /// <exception cref="CustomApplicationException">Thrown when an error occurs while deleting the user.</exception>
        public async Task<bool> DeleteUser(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(email);
                if (user == null) return false;

                await _unitOfWork.Users.DeleteAsync(user.Id);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }

    }

}
