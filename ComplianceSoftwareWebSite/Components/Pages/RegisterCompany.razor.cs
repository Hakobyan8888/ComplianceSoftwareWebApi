using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ComplianceSoftwareWebSite.Components.Pages
{
    public partial class RegisterCompany
    {
        private CompanyModel _companyDetails;
        private List<string> _entityTypes;
        private List<string> _industryNames;
        private List<IndustryType> _industries;
        public RegisterCompany()
        {
            _companyDetails = new CompanyModel();
            _industries = new List<IndustryType>();
            _industryNames = new List<string>();
            _entityTypes = new List<string>();
        }
        [Inject]
        public ICompanyService _companyService { get; set; }

        private void IndustryValueChanged(string selectedItem)
        {
            _companyDetails.BusinessIndustry = _industries.FirstOrDefault(x => x.IndustryName == selectedItem);
        }

        protected override async Task OnInitializedAsync()
        {
            _entityTypes = await _companyService.GetEntityTypes();
            _industries = await _companyService.GetIndustries();
            _industryNames = _industries.Select(x => x.IndustryName).ToList();
            await base.OnInitializedAsync();
        }

        private async void HandleValidSubmit()
        {
            await _companyService.RegisterCompany(_companyDetails);
        }
    }
}