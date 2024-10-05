using ComplianceSoftwareWebSite.Models;

namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<List<IndustryType>> GetIndustries();
        Task<List<string>> GetEntityTypes();
        Task<bool> RegisterCompany(CompanyModel companyModel);
    }
}
