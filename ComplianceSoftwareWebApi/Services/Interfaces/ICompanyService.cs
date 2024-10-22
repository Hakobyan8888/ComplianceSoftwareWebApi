using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> AddCompanyAsync(CompanyDto registerDto, string userId);
        Task<List<string>> GetEntityTypes();
        Task<List<Industry>> GetIndustries();
        Task<Company> GetCompany(string userId);
        Task<IEnumerable<License>> GetRequiredFederalLicenses(string userId);
        Task<IEnumerable<License>> GetRequiredStateLicenses(string userId);
    }
}
