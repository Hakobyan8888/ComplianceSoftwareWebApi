using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ComplianceSoftwareWebSite.Components.Pages.AuthorizedPages
{
    public partial class Licenses
    {
        [Inject]
        public ILicenseService LicenseService { get; set; }

        [Parameter]
        public List<License> LicenseModels { get; set; } = new List<License>();

        protected override async Task OnInitializedAsync()
        {
            LicenseModels = await GetRequiredLicenses();
        }

        private async Task<List<License>> GetRequiredLicenses()
        {
            return await LicenseService.GetRequiredLicenses();
        }
    }
}
