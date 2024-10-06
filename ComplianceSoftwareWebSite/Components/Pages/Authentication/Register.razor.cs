using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ComplianceSoftwareWebSite.Components.Pages.Authentication
{
    public partial class Register
    {
        private RegisterModel _registerModel;
        private List<string> _roles;
        public Register()
        {
            _registerModel = new RegisterModel();
            _roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(x => x.ToString()).ToList();
        }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        [Inject]
        public IAuthService AuthService { get; set; }


        private async Task HandleRegister()
        {
            var response = await AuthService.Register(_registerModel);
            if (response)
            {
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                // handle failed login
            }
        }
    }
}
