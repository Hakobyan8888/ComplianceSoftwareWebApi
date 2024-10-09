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

        public async Task<bool> AddUser(RegisterModel registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/add-user", registerModel);
            if (response != null && response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
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

        public async Task<bool> Register(RegisterModel registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerModel);
            if (response != null && response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task DeleteUser(string email)
        {
            var response = await _httpClient.DeleteAsync($"api/Auth/delete-user-employee?email={email}");
            //if(response != null && response.IsSuccessStatusCode) 
        }
    }
}
