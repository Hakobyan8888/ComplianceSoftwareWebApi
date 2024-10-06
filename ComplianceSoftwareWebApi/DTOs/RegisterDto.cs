using ComplianceSoftwareWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public Roles Role { get; set; } // Owner, Manager, Employee
        public int? CompanyId { get; set; } // Optional for Managers and Employees
    }

}
