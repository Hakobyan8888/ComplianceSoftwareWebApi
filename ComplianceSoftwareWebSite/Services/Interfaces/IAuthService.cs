using ComplianceSoftwareWebSite.Models.Auth;

namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Register();

    }
}
