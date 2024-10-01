using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Services.Interfaces;

namespace ComplianceSoftwareWebSite.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly HttpClient _httpClient;

        public LicenseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<LicenseModel>> GetWorkItemsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<LicenseModel>>("api/Document/library");
            return response ?? new List<LicenseModel>();
        }
    }
}
