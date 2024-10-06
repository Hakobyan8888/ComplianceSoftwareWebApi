using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ComplianceSoftwareWebSite.Components.Pages.AuthorizedPages
{
    public partial class AddUserPage
    {
        private RegisterModel _registerModel;
        private List<string> _roles;
        public AddUserPage()
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
            var response = await AuthService.AddUser(_registerModel);
            if (response)
            {
                _navigationManager.NavigateTo("/users");
            }
            else
            {
                // handle failed login
            }
        }
    }
}