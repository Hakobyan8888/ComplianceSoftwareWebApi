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

        protected override async Task OnInitializedAsync()
        {
            _users = await CompanyService.GetCompanyUsers();
            await base.OnInitializedAsync();
        }

        public void DeleteUser(RegisterModel user)
        {
        }
        public void AddUser()
        {
            _navigationManager.NavigateTo("/add-user");
        }
    }
}