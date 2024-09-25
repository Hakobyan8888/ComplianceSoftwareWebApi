namespace ComplianceSoftwareWebApi.DTOs
{
    public class DocumentUploadDto
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
    }
}
