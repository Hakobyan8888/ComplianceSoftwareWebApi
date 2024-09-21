using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace ComplianceSoftwareWebApi.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Address { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Document> Documents { get; set; }
    }

}
