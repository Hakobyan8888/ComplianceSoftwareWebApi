using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ComplianceSoftwareWebSite.Components.Pages.AuthorizedPages
{
    public partial class UsersPage
    {
        private List<RegisterModel> _users;
        public UsersPage()
        {
            _users = new List<RegisterModel>();
        }


        [Inject]
        public NavigationManager _navigationManager { get; set; }

        [Inject]
        public ICompanyService CompanyService { get; set; }
        [Inject]
        public IAuthService AuthService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUsers();
            await base.OnInitializedAsync();
        }

        public async void DeleteUser(RegisterModel user)
        {
            await AuthService.DeleteUser(user.Email);
            await GetUsers();
            await InvokeAsync(StateHasChanged);
        }

        private async Task GetUsers()
        {
            _users = await CompanyService.GetCompanyUsers();
        }

        public void AddUser()
        {
            _navigationManager.NavigateTo("/add-user");
        }
    }
}