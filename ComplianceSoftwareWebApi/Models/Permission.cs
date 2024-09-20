namespace ComplianceSoftwareWebApi.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public PermissionTypes Name { get; set; } // e.g., "Upload Document", "View Documents", "Manage Compliance"
    }

    public enum PermissionTypes
    {
        AddDocument,
        UpdateDocument,
        RemoveDocument,
        ViewDocuments,
        ManageCompliance,
        AddUser
    }
}
