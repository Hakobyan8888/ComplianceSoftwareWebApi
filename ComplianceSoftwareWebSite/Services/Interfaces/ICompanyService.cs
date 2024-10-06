using ComplianceSoftwareWebSite.Models;
using ComplianceSoftwareWebSite.Models.Auth;

namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<List<IndustryType>> GetIndustries();
        Task<List<string>> GetEntityTypes();
        Task<bool> RegisterCompany(CompanyModel companyModel);
        Task<List<RegisterModel>> GetCompanyUsers();
        Task<CompanyModel> GetCompany();
    }
}
