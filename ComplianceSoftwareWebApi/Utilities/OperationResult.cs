namespace ComplianceSoftwareWebApi.Utilities
{
    public class OperationResult
    {
        public bool Success { get; set; }      // Indicates if the operation was successful
        public string? Message { get; set; }    // Optional message (e.g., error message or success info)
        public Guid? DocumentId { get; set; }  // Optional ID of the document (or other resource), if relevant to the operation

        // Optionally, you can add constructors to easily initialize the object.
        public OperationResult() { }

        public OperationResult(bool success, string? message = null, Guid? documentId = null)
        {
            Success = success;
            Message = message;
            DocumentId = documentId;
        }
    }

}
