namespace ComplianceSoftwareWebSite.Models
{
    public class DocumentVersion
    {
        public int Id { get; set; }
        public int VersionNumber { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UploadedBy { get; set; }
        public byte[] Content { get; set; }
    }
}
