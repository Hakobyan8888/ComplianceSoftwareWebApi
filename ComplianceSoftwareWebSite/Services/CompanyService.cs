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
            //return new List<string>
            //{
            //    "Sole Proprietorship", "Partnership", "Corporation (C-Corp)", "S Corporation (S-Corp)",
            //    "Limited Liability Company (LLC)", "Nonprofit Corporation", "Cooperative (Co-op)",
            //    "Professional Corporation (PC)", "Benefit Corporation (B-Corp)", "Joint Venture",
            //    "Trust", "Private Limited Company (Ltd)", "Public Limited Company (PLC)",
            //    "Holding Company", "Series LLC"
            //};
        }

        public async Task<List<IndustryType>> GetIndustries()
        {
            var response = await _httpClient.GetAsync("api/company/industries");
            if (response != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndustryType>>();
            }
            return null;
            //return new List<IndustryType>
            //{
            //    new IndustryType(11, "Agriculture, Forestry, Fishing and Hunting"),
            //    new IndustryType(21, "Mining, Quarrying, and Oil and Gas Extraction"),
            //    new IndustryType(22, "Utilities"),
            //    new IndustryType(23, "Construction"),
            //    new IndustryType(31, "Manufacturing"),
            //    new IndustryType(42, "Wholesale Trade"),
            //    new IndustryType(44, "Retail Trade"),
            //    new IndustryType(48, "Transportation and Warehousing"),
            //    new IndustryType(51, "Information"),
            //    new IndustryType(52, "Finance and Insurance"),
            //    new IndustryType(53, "Real Estate and Rental and Leasing"),
            //    new IndustryType(54, "Professional, Scientific, and Technical Services"),
            //    new IndustryType(55, "Management of Companies and Enterprises"),
            //    new IndustryType(56, "Administrative and Support and Waste Management and Remediation Services"),
            //    new IndustryType(61, "Educational Services"),
            //    new IndustryType(62, "Health Care and Social Assistance"),
            //    new IndustryType(71, "Arts, Entertainment, and Recreation"),
            //    new IndustryType(72, "Accommodation and Food Services"),
            //    new IndustryType(81, "Other Services (except Public Administration)"),
            //    new IndustryType(92, "Public Administration"),
            //};
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
