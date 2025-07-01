namespace ChurchFlowAPI.DTOs
{
    public class UploadDocumentDto
    {
        public string Title { get; set; }
        public string Description { get; set; } 
        public IFormFile File { get; set; }
    }
}
