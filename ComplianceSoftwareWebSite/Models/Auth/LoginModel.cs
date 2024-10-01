using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebSite.Models.Auth
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
