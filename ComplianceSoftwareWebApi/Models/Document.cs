using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

}
