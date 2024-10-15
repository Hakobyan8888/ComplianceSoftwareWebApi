using ComplianceSoftwareWebApi.Models;

namespace ComplianceSoftwareWebApi.DTOs
{
    public class CompanyDto
    {
        public string EntityType { get; set; }
        public string StateOfFormation { get; set; }
        public string BusinessName { get; set; }
        public int BusinessIndustryCode { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public DateTime? EntityFormationDate { get; set; }
    }
}
