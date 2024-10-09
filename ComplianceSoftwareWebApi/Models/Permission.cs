using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public PermissionTypes PermissionType { get; set; }
        public string PermissionName { get; set; } // e.g., "Upload Document", "View Documents", "Manage Compliance"
    }

    public enum PermissionTypes
    {
        AddDocument,
        UpdateDocument,
        RemoveDocument,
        ViewDocuments,
        ManageCompliance,
        AddUser,
        EditUser,
    }
}
