using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Server.IIS;

namespace ComplianceSoftwareWebSite.Components.Pages.Auth
{
    public partial class Login
    {
        private LoginModel loginModel = new LoginModel();
        [Inject]
        public AuthStateProviderService _stateProviderService { get; set; }
        [Inject]
        public HttpClient _httpClient { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        private static bool _isFirstTime = true;
        protected override void OnInitialized()
        {
            if (!_isFirstTime)
            {
                base.OnInitialized();
                return;
            }
            _stateProviderService.SetDummyAuth(string.Empty);
            base.OnInitialized();
            _isFirstTime = false;
        }

        private async Task HandleLogin()
        {
            var response = await new HttpClient() { BaseAddress = new Uri("https://localhost:7216/") }.PostAsJsonAsync("api/Auth/login", loginModel);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                _stateProviderService.MarkUserAsAuthenticated(result.Token);
                //HttpContextAccessory.HttpContext.Session.SetString("AuthToken2", result.Token);
                _navigationManager.NavigateTo("/dashboard");
            }
            else
            {
                // handle failed login
            }
        }
    }
}
