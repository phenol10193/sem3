namespace sem3.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int SupplierId { get; set; }
        public int CustomerId { get; set; }
        public string? Comment { get; set; }
        public string? FirstName { get; set; }
        public string? UrlImage { get; set; }
        public DateTime FeedbackDate { get; set; }
    }
}
