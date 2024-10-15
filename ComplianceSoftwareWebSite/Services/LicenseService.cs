using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Services.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ComplianceSoftwareWebSite.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly HttpClient _httpClient;

        public LicenseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DocumentModel>> GetWorkItemsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<DocumentModel>>("api/Document/library");
            return response ?? new List<DocumentModel>();
        }

        public async Task<List<License>> GetRequiredLicenses()
        {
            var response = await _httpClient.GetAsync("api/Company/required-licenses");
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<License>>();
            }
            return new List<License>();
        }
    }
}
