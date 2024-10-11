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

        public async Task<CompanyModel> GetCompany()
        {

            var response = await _httpClient.GetAsync("api/company/get");
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CompanyModel>();
            }
            return null;
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

        public async Task<List<Industry>> GetIndustries()
        {
            var response = await _httpClient.GetAsync("api/company/industries");
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Industry>>();
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


        public async Task<List<RegisterModel>> GetCompanyUsers()
        {
            var response = await _httpClient.GetAsync("api/Auth/get-users");
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response?.Content?.ReadFromJsonAsync<List<RegisterModel>>();
            }
            return null;
        }
    }
}
