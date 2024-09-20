namespace ComplianceSoftwareWebApi.DTOs
{
    public class DocumentDto
    {
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FilePath { get; set; }  // For storing file locations if documents are files
    }
}
