using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSqlHelper.Models
{
    public class Industry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string IndustryType { get; set; }

        public int IndustryCode { get; set; }

        public string SectorCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string IndustryName { get; set; }

        // Navigation property for related licenses
        public virtual ICollection<License> Licenses { get; set; } = new List<License>();
    }
}
