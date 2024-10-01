using ComplianceSoftwareWebSite.Models;

namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface ILicenseService
    {
        Task<List<LicenseModel>> GetWorkItemsAsync();
    }
}
