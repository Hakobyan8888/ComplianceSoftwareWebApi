using ComplianceSoftwareWebApi.Data;
using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Repositories.Interfaces;
using ComplianceSoftwareWebApi.Services.Interfaces;
using ComplianceSoftwareWebApi.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _uniUnitOfWork;
        private readonly IUserService _userService; // to get user details
        private readonly IPermissionService _permissionService; // for permission checks

        public DocumentService(IUnitOfWork unitOfWork, IUserService userService, IPermissionService permissionService)
        {
            _uniUnitOfWork = unitOfWork;
            _userService = userService;
            _permissionService = permissionService;
        }

        public async Task<List<Document>> GetCompanyDocumentsAsync(int companyId)
        {
            return (await _uniUnitOfWork.Documents.GetDocumentsByCompanyIdAsync(companyId)).ToList();
        }

        public async Task<Document> AddDocumentAsync(DocumentDto dto, string userId)
        {
            var user = await _uniUnitOfWork.Users.GetByIdAsync(userId);
            var document = new Document
            {
                CompanyId = user.Company.Id,
                Title = dto.Title,
                Content = dto.Content,
                FilePath = dto.FilePath,
                CreatedAt = DateTime.UtcNow
            };

            await _uniUnitOfWork.Documents.AddAsync(document);
            await _uniUnitOfWork.CompleteAsync();
            return document;
        }

        public async Task<Document> UpdateDocumentAsync(int documentId, DocumentDto dto)
        {
            var document = await _uniUnitOfWork.Documents.GetByIdAsync(documentId);
            if (document == null) throw new Exception("Document not found");

            document.Title = dto.Title;
            document.Content = dto.Content;
            document.FilePath = dto.FilePath;
            document.UpdatedAt = DateTime.UtcNow;

            await _uniUnitOfWork.Documents.UpdateAsync(document);
            await _uniUnitOfWork.CompleteAsync();
            return document;
        }

        public async Task RemoveDocumentAsync(int documentId)
        {
            var document = await _uniUnitOfWork.Documents.GetByIdAsync(documentId);
            if (document == null) throw new Exception("Document not found");

            await _uniUnitOfWork.Documents.DeleteAsync(documentId);
            await _uniUnitOfWork.CompleteAsync();
        }
    }
}
