
using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Repositories.Interfaces
{
    public interface ILicenseRepository : IRepository<License>
    {
        // Add custom methods for the License repository if needed
        Task<IEnumerable<License>> GetLicensesByIndustryCode(int code);

        Task<IEnumerable<License>> GetLicensesByIndustryCodeAndState(int code, string stateCode);
    }
}
