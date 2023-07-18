namespace sem3.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int CustomerId { get; set; }
        public string? CContent { get; set; }
        public DateTime SentDate { get; set; }
        public bool Flag { get; set; }
    }
}
