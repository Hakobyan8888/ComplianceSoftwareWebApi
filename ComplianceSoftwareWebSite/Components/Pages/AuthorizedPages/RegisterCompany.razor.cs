using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ComplianceSoftwareWebSite.Components.Pages.AuthorizedPages
{
    public partial class RegisterCompany
    {
        private CompanyModel _companyDetails;
        private List<string> _entityTypes;
        private List<string> _industryNames;
        private List<Industry> _industries;
        public RegisterCompany()
        {
            _companyDetails = new CompanyModel();
            _industries = new List<Industry>();
            _industryNames = new List<string>();
            _entityTypes = new List<string>();
        }
        [Inject]
        public ICompanyService _companyService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        private void IndustryValueChanged(string selectedItem)
        {
            _companyDetails.BusinessIndustry = _industries.FirstOrDefault(x => x.IndustryType == selectedItem);
        }

        protected override async Task OnInitializedAsync()
        {
            var company = await _companyService.GetCompany();
            if (company != null)
            {
                _navigationManager.NavigateTo("/dashboard");
            }
            _entityTypes = await _companyService.GetEntityTypes();
            _industries = await _companyService.GetIndustries();
            _industryNames = _industries.Select(x => x.IndustryType).ToList();
            await base.OnInitializedAsync();
        }

        private async void HandleValidSubmit()
        {
            var response = await _companyService.RegisterCompany(_companyDetails);
            if (response)
                _navigationManager.NavigateTo("/dashboard");
        }
    }
}