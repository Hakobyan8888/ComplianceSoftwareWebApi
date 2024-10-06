using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.VisualBasic;

namespace ComplianceSoftwareWebSite.Components.Pages.Authentication
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

        [Inject]
        public IAuthService AuthService { get; set; }
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
            var response = await AuthService.Login(loginModel);
            if (response != null)
            {
                await _stateProviderService.MarkUserAsAuthenticated(response.Token);
                _navigationManager.NavigateTo("/company");
            }
            else
            {
                // handle failed login
            }
        }
    }
}
