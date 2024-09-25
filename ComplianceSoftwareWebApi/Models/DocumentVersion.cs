using System.ComponentModel.DataAnnotations;

namespace ComplianceSoftwareWebApi.Models
{
    public class DocumentVersion
    {
        [Key]
        public int Id { get; set; }
        public int VersionNumber { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UploadedBy { get; set; }
        public byte[] Content { get; set; }
    }
}
