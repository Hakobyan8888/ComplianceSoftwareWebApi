using ComplianceSoftwareWebApi.DTOs;
using ComplianceSoftwareWebApi.Models;
using ComplianceSoftwareWebApi.Services;
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

        public DocumentController(IDocumentService documentService,
            IPermissionService permissionService,
            IUserService userService)
        {
            _documentService = documentService;
            _userService = userService;
            _permissionService = permissionService;
        }

        // 1. Upload a Document
        [HttpPost("upload")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadDto documentUpload)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var user = await _userService.GetUserById(userId);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddDocument) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }


            if (documentUpload.File == null || documentUpload.File.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            var result = await _documentService.UploadDocumentAsync(documentUpload, Convert.ToInt32(user.CompanyId));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.DocumentId);
        }

        // 2. Get List of Documents (Paginated)
        [HttpGet("library")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> GetDocumentLibrary([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var user = await _userService.GetUserById(userId);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }
            var documents = await _documentService.GetDocumentLibraryAsync(page, pageSize, Convert.ToInt32(user.CompanyId));
            return Ok(documents);
        }

        // 3. Get Specific Document by ID
        [HttpGet("{documentId}")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> GetDocument(Guid documentId)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var user = await _userService.GetUserById(userId);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }
            var document = await _documentService.GetDocumentByIdAsync(documentId, Convert.ToInt32(user.CompanyId));
            if (document == null)
            {
                return NotFound("Document not found.");
            }

            return Ok(document);
        }

        // 4. Download a Document
        [HttpGet("{documentId}/download")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> DownloadDocument(Guid documentId)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var user = await _userService.GetUserById(userId);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }

            var document = await _documentService.DownloadDocumentAsync(documentId, Convert.ToInt32(user.CompanyId));
            if (document == null)
            {
                return NotFound("Document not found.");
            }

            return File(document.Content, document.ContentType, document.FileName);
        }

        // 5. Update a Document (new version)
        [HttpPut("{documentId}/update")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> UpdateDocument(Guid documentId, [FromForm] DocumentUploadDto documentUpload)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var user = await _userService.GetUserById(userId);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }
            var result = await _documentService.UpdateDocumentAsync(documentId, documentUpload, Convert.ToInt32(user.CompanyId));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var updatedDocument = await _documentService.GetDocumentByIdAsync(documentId, Convert.ToInt32(user.CompanyId));
            return Ok(updatedDocument);
        }

        // 6. Delete a Document
        [HttpDelete("{documentId}")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> DeleteDocument(Guid documentId)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var user = await _userService.GetUserById(userId);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }

            var result = await _documentService.DeleteDocumentAsync(documentId, Convert.ToInt32(user.CompanyId));
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok("Document deleted successfully.");
        }

        // 7. Get Document Versions
        [HttpGet("{documentId}/versions")]
        [Authorize(Roles = "Owner,Manager,Employee")]
        public async Task<IActionResult> GetDocumentVersions(Guid documentId)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var user = await _userService.GetUserById(userId);

            // Check if the user has permission to add new users
            if (!await _permissionService.HasPermissionAsync(userId, PermissionTypes.AddUser) && !User.IsInRole("Owner"))
            {
                return Forbid("You do not have permission to add users.");
            }

            var versions = await _documentService.GetDocumentVersionsAsync(documentId, Convert.ToInt32(user.CompanyId));
            if (versions == null || !versions.Any())
            {
                return NotFound("No versions found for the document.");
            }

            return Ok(versions);
        }
    }
}
