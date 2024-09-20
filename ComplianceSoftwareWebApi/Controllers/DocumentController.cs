using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceSoftwareWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IPermissionService _permissionService;

        public DocumentController(IDocumentService documentService, IPermissionService permissionService)
        {
            _documentService = documentService;
            _permissionService = permissionService;
        }

        // Get all documents for a company
        [HttpGet("{companyId}")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> GetCompanyDocuments(int companyId)
        {
            if (!await _permissionService.HasPermissionAsync(User, PermissionTypes.ViewDocuments))
            {
                return Forbid("You do not have permission to add documents.");
            }
            var documents = await _documentService.GetCompanyDocumentsAsync(companyId);
            return Ok(documents);
        }

        // Add a new document (only Owner or users with permission can add)
        [HttpPost("add")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> AddDocument(DocumentDto dto)
        {
            if (!await _permissionService.HasPermissionAsync(User, PermissionTypes.AddDocument))
            {
                return Forbid("You do not have permission to add documents.");
            }

            var document = await _documentService.AddDocumentAsync(dto);
            return Ok(document);
        }

        // Update an existing document (only Owner or users with permission can update)
        [HttpPut("update/{documentId}")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> UpdateDocument(int documentId, DocumentDto dto)
        {
            if (!await _permissionService.HasPermissionAsync(User, PermissionTypes.UpdateDocument))
            {
                return Forbid("You do not have permission to update documents.");
            }

            var updatedDocument = await _documentService.UpdateDocumentAsync(documentId, dto);
            return Ok(updatedDocument);
        }

        // Remove a document (only Owner or users with permission can remove)
        [HttpDelete("remove/{documentId}")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            if (!await _permissionService.HasPermissionAsync(User, PermissionTypes.RemoveDocument))
            {
                return Forbid("You do not have permission to remove documents.");
            }

            await _documentService.RemoveDocumentAsync(documentId);
            return NoContent();
        }
    }
}
