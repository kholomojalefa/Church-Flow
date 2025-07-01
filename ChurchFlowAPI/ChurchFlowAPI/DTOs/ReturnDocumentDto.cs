namespace ChurchFlowAPI.DTOs
{
    public class ReturnDocumentDto
    {
        public int Id { get; set; }                 // Public ID for reference
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }        // URL path to the uploaded file
        public DateTime UploadedAt { get; set; }
        public string UploadedByName { get; set; }  // Display name of the user
    }
}
