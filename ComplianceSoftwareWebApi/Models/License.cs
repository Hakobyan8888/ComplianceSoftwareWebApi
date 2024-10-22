using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class License
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string LicenseName { get; set; }

        [Required]
        [MaxLength(200)]
        public string IssuingAgency { get; set; }

        [MaxLength(500)]
        public string IssuingAgencyLink { get; set; }
        public string StateCode { get; set; }

        // Foreign key reference to Industry
        public int IndustryId { get; set; }
        public virtual Industry Industry { get; set; }
    }
}
