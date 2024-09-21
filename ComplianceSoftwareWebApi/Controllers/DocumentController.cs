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
        private readonly IUserService _userService;


        public DocumentController(IDocumentService documentService, IPermissionService permissionService, IUserService userService)
        {
            _documentService = documentService;
            _permissionService = permissionService;
            _userService = userService;
        }

        // Get all documents for a company
        [HttpGet("{companyId}")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> GetCompanyDocuments(int companyId)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.ViewDocuments))
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
            var userId = _userService.GetUserIdFromClaims(User);
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddDocument))
            {
                return Forbid("You do not have permission to add documents.");
            }

            var document = await _documentService.AddDocumentAsync(dto, userId);
            return Ok(document);
        }

        // Update an existing document (only Owner or users with permission can update)
        [HttpPut("update/{documentId}")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> UpdateDocument(int documentId, DocumentDto dto)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.UpdateDocument))
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
            var userId = _userService.GetUserIdFromClaims(User);
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.RemoveDocument))
            {
                return Forbid("You do not have permission to remove documents.");
            }

            await _documentService.RemoveDocumentAsync(documentId);
            return NoContent();
        }
    }
}
