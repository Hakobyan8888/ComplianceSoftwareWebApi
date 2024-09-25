using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Document>> GetDocumentsByCompanyIdAsync(int companyId)
        {
            return await _context.Documents
                .Where(d => d.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Document> GetDocumentVersionsAsync(Guid documentId, int companyId)
        {
            return await _context.Documents.Include(d => d.Versions).FirstOrDefaultAsync(d => d.Id == documentId && d.CompanyId == companyId);
        }


    }

}
