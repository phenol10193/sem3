using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string? CContent { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public DateTime SentDate { get; set; }
        public bool Flag { get; set; }
        public  Customer? Customer { get; set; } 

        public  Supplier? Supplier { get; set; } 
    }
}
