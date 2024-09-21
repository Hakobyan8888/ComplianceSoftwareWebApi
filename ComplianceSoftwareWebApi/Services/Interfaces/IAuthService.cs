using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto, List<string> roles);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserToCompanyAsync(RegisterDto dto);
    }

}
