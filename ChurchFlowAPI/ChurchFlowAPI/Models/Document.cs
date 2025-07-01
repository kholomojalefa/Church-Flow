namespace ChurchFlowAPI.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string UploadedById { get; set; }
        public ApplicationUser UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
