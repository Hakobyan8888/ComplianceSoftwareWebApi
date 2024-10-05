using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace ComplianceSoftwareWebApi.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string EntityType { get; set; }
        public string StateOfFormation { get; set; }
        public string BusinessName { get; set; }
        public int BusinessIndustryCode { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Document> Documents { get; set; }
    }

}
