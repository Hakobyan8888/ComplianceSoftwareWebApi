using ComplianceSoftwareWebSite.Api_Clients;
using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;

namespace ComplianceSoftwareWebSite.Components.Pages.AuthorizedPages
{
    public partial class RegisterCompany
    {
        private CompanyModel _companyDetails;
        private List<string> _entityTypes;
        private List<string> _industryNames;
        private List<Industry> _industries;

        private Dictionary<string, string> _states;
        private Dictionary<string, string> _counties;
        private Dictionary<string, string> _cities;

        [Inject]
        private CensusApiClient _censusApiClient { get; set; }

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
            _companyDetails.BusinessIndustryCode = _industries.FirstOrDefault(x => x.IndustryType == selectedItem).IndustryCode;
        }

        private async void StateValueChanged(string state)
        {
            _companyDetails.StateOfFormation = state;
            _counties = await _censusApiClient.GetCountiesByStateFips(_states[state]);
            await InvokeAsync(StateHasChanged);
        }

        private async void CountyValueChanged(string county)
        {
            _companyDetails.CountyOfFormation = county;
            _cities = await _censusApiClient.GetCitiesByStateAndCountyFips(_states[_companyDetails.StateOfFormation], _counties[county]);
            await InvokeAsync(StateHasChanged);
        }

        private void CityValueChanged(string city)
        {
            _companyDetails.City = city;
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
            _states = await _censusApiClient.GetStates();
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