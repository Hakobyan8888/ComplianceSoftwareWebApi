using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class UserPermission
    {
        public string UserId { get; set; }
        public User User { get; set; }

        [Key]
        public int Id { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }

}
