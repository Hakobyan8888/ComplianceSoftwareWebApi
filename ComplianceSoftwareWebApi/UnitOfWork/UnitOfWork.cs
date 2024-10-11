using ComplianceSoftwareWebApi.Repositories.Interfaces;
using ComplianceSoftwareWebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using ComplianceSoftwareWebApi.Data;

namespace ComplianceSoftwareWebApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository Users { get; private set; }
        public IDocumentRepository Documents { get; private set; }
        public IPermissionRepository Permissions { get; private set; }
        public IUserPermissionRepository UserPermissions { get; private set; }

        public ICompanyRepository Companies { get; private set; }

        public IIndustryTypeRepository IndustryTypes { get; private set; }

        public IIndustryRepository Industries { get; private set; }
        public ILicenseRepository Licenses { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Documents = new DocumentRepository(context);
            Permissions = new PermissionRepository(context);
            UserPermissions = new UserPermissionRepository(context);
            Companies = new CompanyRepository(context);
            IndustryTypes = new IndustryTypeRepository(context);
            Industries = new IndustryRepository(context);
            Licenses = new LicenseRepository(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
