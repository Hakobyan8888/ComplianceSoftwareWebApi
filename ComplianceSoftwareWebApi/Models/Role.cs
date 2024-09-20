namespace ComplianceSoftwareWebApi.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public RoleNames Name { get; set; } // Owner, Manager, Employee
    }

    public enum RoleNames
    {
        Owner,
        Manager,
        Employee
    }
}
