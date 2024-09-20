using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserService _userService; // to get user details
        private readonly IPermissionService _permissionService; // for permission checks

        public DocumentService(IDocumentRepository documentRepository, IUserService userService, IPermissionService permissionService)
        {
            _documentRepository = documentRepository;
            _userService = userService;
            _permissionService = permissionService;
        }

        public async Task<List<Document>> GetCompanyDocumentsAsync(int companyId)
        {
            return (await _documentRepository.GetDocumentsByCompanyIdAsync(companyId)).ToList();
        }

        public async Task<Document> AddDocumentAsync(DocumentDto dto)
        {
            var document = new Document
            {
                CompanyId = dto.CompanyId,
                Title = dto.Title,
                Content = dto.Content,
                FilePath = dto.FilePath,
                CreatedAt = DateTime.UtcNow
            };

            await _documentRepository.AddAsync(document);
            return document;
        }

        public async Task<Document> UpdateDocumentAsync(int documentId, DocumentDto dto)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null) throw new Exception("Document not found");

            document.Title = dto.Title;
            document.Content = dto.Content;
            document.FilePath = dto.FilePath;
            document.UpdatedAt = DateTime.UtcNow;

            await _documentRepository.UpdateAsync(document);
            return document;
        }

        public async Task RemoveDocumentAsync(int documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null) throw new Exception("Document not found");

            await _documentRepository.DeleteAsync(documentId);
        }
    }
}
