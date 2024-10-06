namespace ComplianceSoftwareWebSite.Models.Auth
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; } // Owner, Manager, Employee
    }

    public enum Roles
    {
        Owner,
        Manager,
        Employee
    }
}
