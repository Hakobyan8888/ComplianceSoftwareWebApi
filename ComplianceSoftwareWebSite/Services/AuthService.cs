using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services.Interfaces;

namespace ComplianceSoftwareWebSite.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginModel);
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResult>();
            }
            return null;
        }

        public Task Register()
        {
            return Task.CompletedTask;
        }
    }
}
