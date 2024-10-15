using ComplianceSoftwareWebSite.Models;

namespace ComplianceSoftwareWebSite.Services.Interfaces
{
    public interface ILicenseService
    {
        Task<List<DocumentModel>> GetWorkItemsAsync();
        Task<List<License>> GetRequiredLicenses();
    }
}
