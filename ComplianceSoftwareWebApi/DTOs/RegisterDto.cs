using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.DTOs
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; } // Owner, Manager, Employee
        public int? CompanyId { get; set; } // Optional for Managers and Employees
    }

}
