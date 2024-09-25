using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Utilities;

namespace ComplianceSoftwareWebApi.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<OperationResult> UploadDocumentAsync(DocumentUploadDto documentUpload, int companyId);
        Task<OperationResult> UpdateDocumentAsync(Guid documentId, DocumentUploadDto documentUpload, int companyId);
        Task<OperationResult> DeleteDocumentAsync(Guid documentId, int companyId);
        Task<DocumentDto> GetDocumentByIdAsync(Guid documentId, int companyId);
        Task<IEnumerable<DocumentDto>> GetDocumentLibraryAsync(int page, int pageSize, int companyId);
        Task<Document> DownloadDocumentAsync(Guid documentId, int companyId);
        Task<IEnumerable<DocumentVersionDto>> GetDocumentVersionsAsync(Guid documentId, int companyId);
    }


}
