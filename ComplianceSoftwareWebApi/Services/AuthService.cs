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

        public async Task<User> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Role = registerDto.Role,
                CompanyId = registerDto.CompanyId,
                PasswordHash = _passwordHasher.HashPassword(null, registerDto.Password)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return user;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
            if (user == null || _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginDto.Password) != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return _jwtTokenGenerator.GenerateToken(user);
        }

        public async Task AddUserToCompanyAsync(RegisterDto dto)
        {
            // Check if the user already exists by email
            var existingUser = await GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            // Create a new user
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

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetByEmailAsync(email);
        }
    }

}
