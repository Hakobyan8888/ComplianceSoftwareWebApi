using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<List<Document>> GetCompanyDocumentsAsync(int companyId);
        Task<Document> AddDocumentAsync(DocumentDto dto, string userId);
        Task<Document> UpdateDocumentAsync(int documentId, DocumentDto dto);
        Task RemoveDocumentAsync(int documentId);
    }

}
