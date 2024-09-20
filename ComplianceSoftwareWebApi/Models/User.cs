using Microsoft.AspNetCore.Identity;

namespace ComplianceSoftwareWebApi.Models
{
    public class User : IdentityUser
    {
        public Roles Role { get; set; } // Owner, Manager, Employee

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        // Permissions granted to the user
        public ICollection<UserPermission> UserPermissions { get; set; }
    }

    public enum Roles
    {
        Owner,
        Manager,
        Employee
    }

}
