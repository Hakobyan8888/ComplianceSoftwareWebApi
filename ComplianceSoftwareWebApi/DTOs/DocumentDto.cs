namespace ComplianceSoftwareWebApi.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UploadedBy { get; set; }
        public List<DocumentVersionDto> Versions { get; set; }
    }
}
