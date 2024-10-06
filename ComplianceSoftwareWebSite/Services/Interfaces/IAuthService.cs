using ComplianceSoftwareWebSite.Models.Auth;

namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task<bool> Register(RegisterModel registerModel);
        Task<bool> AddUser(RegisterModel registerModel);

    }
}
