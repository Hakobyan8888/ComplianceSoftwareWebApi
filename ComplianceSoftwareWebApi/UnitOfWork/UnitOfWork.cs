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

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Documents = new DocumentRepository(context);
            Permissions = new PermissionRepository(context);
            UserPermissions = new UserPermissionRepository(context);
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
