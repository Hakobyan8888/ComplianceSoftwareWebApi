using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class Industry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string IndustryType { get; set; }

        public int IndustryCode { get; set; }

        public int SectorCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string IndustryName { get; set; }

        // Navigation property for related licenses
        public virtual ICollection<License> Licenses { get; set; }
    }
}
