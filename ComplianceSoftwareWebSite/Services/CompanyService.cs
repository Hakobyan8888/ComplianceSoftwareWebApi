using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services.Interfaces;

namespace ComplianceSoftwareWebSite.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;
        public CompanyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetEntityTypes()
        {

            var response = await _httpClient.GetAsync("api/company/entity-types");
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<string>>();
            }
            return null;
        }

        public async Task<List<IndustryType>> GetIndustries()
        {
            var response = await _httpClient.GetAsync("api/company/industries");
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndustryType>>();
            }
            return null;
        }

        public async Task<bool> RegisterCompany(CompanyModel companyModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Company/create", companyModel);
            if (response != null && response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
