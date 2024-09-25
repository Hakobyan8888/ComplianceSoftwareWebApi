using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Repositories.Interfaces
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<IEnumerable<Document>> GetDocumentsByCompanyIdAsync(int companyId);
        Task<Document> GetDocumentVersionsAsync(Guid documentId, int companyId);
    }

}
