using ComplianceSoftwareWebSite.Models;
using Microsoft.AspNetCore.Components;

namespace ComplianceSoftwareWebSite.Components.Pages.AuthorizedPages
{
    public partial class Licenses
    {
        [Parameter]
        public List<LicenseModel> LicenseModels { get; set; } = new List<LicenseModel>
        {
            new LicenseModel()
            {
                Id = Guid.NewGuid(),
                Title = "My New Bug",
                FileName = "FileName",
                CreatedDate = new DateTime(),
            },
            new LicenseModel()
            {
                Id = Guid.NewGuid(),
                Title = "My New Bug",
                FileName = "FileName",
                CreatedDate = new DateTime(),
            },
        };


        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
    }
}
