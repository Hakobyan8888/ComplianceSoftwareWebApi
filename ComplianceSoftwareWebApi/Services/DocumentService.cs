using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using ComplianceSoftwareWebApi.Services.Interfaces;
using ComplianceSoftwareWebApi.UnitOfWork;
using ComplianceSoftwareWebApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Upload a new document
        public async Task<OperationResult> UploadDocumentAsync(DocumentUploadDto documentUpload, int companyId)
        {
            var document = new Document
            {
                Title = documentUpload.Title,
                FileName = documentUpload.File.FileName,
                ContentType = documentUpload.File.ContentType,
                FileSize = documentUpload.File.Length,
                CreatedDate = DateTime.UtcNow,
                UploadedByEmail = "UserName", // Adjust as needed based on your auth
                CompanyId = companyId,
            };

            using (var stream = new MemoryStream())
            {
                await documentUpload.File.CopyToAsync(stream);
                document.Content = stream.ToArray();
            }

            await _unitOfWork.Documents.AddAsync(document);
            await _unitOfWork.CompleteAsync();

            return new OperationResult { Success = true, DocumentId = document.Id };
        }

        // Update document with a new version
        public async Task<OperationResult> UpdateDocumentAsync(Guid documentId, DocumentUploadDto documentUpload, int companyId)
        {
            var document = await _unitOfWork.Documents.GetDocumentVersionsAsync(documentId, companyId);
            if (document == null)
            {
                return new OperationResult { Success = false, Message = "Document not found." };
            }

            var newVersion = new DocumentVersion
            {
                VersionNumber = document.Versions.Any() ? document.Versions.Max(v => v.VersionNumber) + 1 : 1,
                UploadedDate = DateTime.UtcNow,
                UploadedBy = "UserName", // Adjust based on auth
            };

            using (var stream = new MemoryStream())
            {
                await documentUpload.File.CopyToAsync(stream);
                newVersion.Content = stream.ToArray();
            }

            document.Versions.Add(newVersion);
            await _unitOfWork.CompleteAsync();

            return new OperationResult { Success = true, DocumentId = document.Id };
        }

        // Delete a document
        public async Task<OperationResult> DeleteDocumentAsync(Guid documentId, int companyId)
        {
            var document = await _unitOfWork.Documents.GetDocumentVersionsAsync(documentId, companyId);
            if (document == null)
            {
                return new OperationResult { Success = false, Message = "Document not found." };
            }

            //await _unitOfWork.Documents.DeleteAsync(document.d);
            //await _unitOfWork.CompleteAsync();

            return new OperationResult { Success = true };
        }

        // Get document by ID
        public async Task<DocumentDto> GetDocumentByIdAsync(Guid documentId, int companyId)
        {
            var document = await _unitOfWork.Documents.GetDocumentVersionsAsync(documentId, companyId);
            if (document == null)
            {
                return null;
            }

            return new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                FileName = document.FileName,
                ContentType = document.ContentType,
                FileSize = document.FileSize,
                CreatedDate = document.CreatedDate,
                UploadedBy = document.UploadedByEmail,
                Versions = document.Versions.Select(v => new DocumentVersionDto
                {
                    VersionNumber = v.VersionNumber,
                    UploadedDate = v.UploadedDate,
                    UploadedBy = v.UploadedBy
                }).ToList()
            };
        }

        // Get paginated list of documents
        public async Task<IEnumerable<DocumentDto>> GetDocumentLibraryAsync(int page, int pageSize, int companyId)
        {
            var documents = (await _unitOfWork.Documents.GetDocumentsByCompanyIdAsync(companyId)).ToList()
                .OrderByDescending(d => d.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return documents.Select(d => new DocumentDto
            {
                Id = d.Id,
                Title = d.Title,
                FileName = d.FileName,
                ContentType = d.ContentType,
                FileSize = d.FileSize,
                CreatedDate = d.CreatedDate,
                UploadedBy = d.UploadedByEmail
            }).ToList();
        }

        // Download a document
        public async Task<Document> DownloadDocumentAsync(Guid documentId, int companyId)
        {
            var document = await _unitOfWork.Documents.GetDocumentVersionsAsync(documentId, companyId);
            if (document == null)
            {
                return null;
            }

            return document;
        }

        // Get all versions of a document
        public async Task<IEnumerable<DocumentVersionDto>> GetDocumentVersionsAsync(Guid documentId, int companyId)
        {
            var document = await _unitOfWork.Documents.GetDocumentVersionsAsync(documentId, companyId);
            if (document == null)
            {
                return null;
            }

            return document.Versions.Select(v => new DocumentVersionDto
            {
                VersionNumber = v.VersionNumber,
                UploadedDate = v.UploadedDate,
                UploadedBy = v.UploadedBy
            }).ToList();
        }
    }

}
