using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public byte[] Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UploadedByEmail { get; set; }
        public ICollection<DocumentVersion> Versions { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

}
