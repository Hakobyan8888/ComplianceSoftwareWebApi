using ComplianceSoftwareWebApi.Repositories.Interfaces;

namespace ComplianceSoftwareWebApi.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IDocumentRepository Documents { get; }
        IPermissionRepository Permissions { get; }
        IUserPermissionRepository UserPermissions { get; }
        ICompanyRepository Companies { get; }
        IIndustryTypeRepository IndustryTypes { get; }
        Task<int> CompleteAsync();
    }

}
